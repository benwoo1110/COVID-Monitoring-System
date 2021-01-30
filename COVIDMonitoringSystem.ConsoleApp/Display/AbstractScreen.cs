//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public abstract class AbstractScreen
    {
        public abstract string Name { get; }
        
        public ConsoleDisplayManager DisplayManager { get; }
        public List<Element> ElementList { get; }
        public List<SelectableElement> CachedSelectableElement { get; }
        public int SelectedIndex { get; set; }
        public SelectableElement SelectedElement { get; private set; }
        public List<Element> UpdateQueue { get; }
        public bool Active { get; private set; }

        public AbstractScreen(ConsoleDisplayManager displayManager)
        {
            DisplayManager = displayManager;
            ElementList = new List<Element>();
            CachedSelectableElement = new List<SelectableElement>();
            UpdateQueue = new List<Element>();
            AddElementFields();
            AddButtonMethods();
        }

        private void AddElementFields()
        {
            var elementList = ReflectHelper.GetFieldsOfType<Element>(this);
            foreach (var (name, element) in elementList)
            {
                if (string.IsNullOrEmpty(element.Name))
                {
                    element.Name = name;
                }
                AddElement(element);
            }
        }

        private void AddButtonMethods()
        {
            var buttonMethodsMap = ReflectHelper.GetMethodWithAttribute<OnClick>(this);
            foreach (var (method, clickAttr) in buttonMethodsMap)
            {
                var button = FindElementOfType<Button>(clickAttr.ButtonName);
                if (button == null)
                {
                    throw new InvalidOperationException($"Button {clickAttr.ButtonName} not found.");
                }

                button.MethodRunner = new ActionMethod(method);
            }

            //TODO: Make this better
            var inputMethodMap = ReflectHelper.GetMethodWithAttribute<OnEnterInput>(this);
            foreach (var (method, clickAttr) in inputMethodMap)
            {
                var input = FindElementOfType<Input>(clickAttr.InputName);
                if (input == null)
                {
                    throw new InvalidOperationException($"Button {clickAttr.InputName} not found.");
                }

                input.MethodRunner = new ActionMethod(method);
            }
        }
        
        public void AddElement(Element element)
        {
            element.TargetScreen = this;
            ElementList.Add(element);

            if (element is SelectableElement selectableElement)
            {
                CachedSelectableElement.Add(selectableElement);
            }
        }

        public virtual void PrePassData(object data)
        {
            
        }

        public virtual void PreLoad()
        {
            
        }

        public virtual void Load()
        {
            if (Active)
            {
                throw new InvalidOperationException("Nested screen stacking not supported.");
            }

            Active = true;
            Render();
            SelectDefault();
        }

        public virtual void OnView()
        {
        }

        public virtual void OnClose()
        {
        }

        public virtual void Unload()
        {
            Active = false;
            ClearSelection();
            FindAllElementOfType<TextElement>()
                .FindAll(element => element.ClearOnExit)
                .ForEach(element => element.ClearText());
        }

        public virtual void Closed()
        {
        }

        public virtual void Render()
        {
            CHelper.Clear();
            UpdateQueue.Clear();
            foreach (var element in ElementList.Where(element => !element.Hidden))
            {
                element.Render();
            }
            ColourSelector.Default();
            SetCursor();
        }

        public void Update()
        {
            var queueSnapshot = new List<Element>(UpdateQueue);
            UpdateQueue.Clear();
            
            queueSnapshot.ForEach(element => element.Render());
            ColourSelector.Default();
            SetCursor();
        }

        public void SelectDefault()
        {
            ClearSelection();
            ShiftSelectionBy(0);
        }

        public void SelectNext()
        {
            ShiftSelectionBy(1);
        }

        public void SelectPrevious()
        {
            ShiftSelectionBy(-1);
        }

        private void ShiftSelectionBy(int by)
        {
            var selectableElements = GetAllSelectableElements();
            if (selectableElements.Count == 0)
            {
                return;
            }

            var selectedIndex = (SelectedElement == null) 
                ? 0 
                : selectableElements.IndexOf(SelectedElement);

            var newSelectedElement = selectableElements[CoreHelper.Mod(selectedIndex + by, selectableElements.Count)];
            if (newSelectedElement == null)
            {
                return;
            }

            if (SelectedElement != null)
            {
                SelectedElement.Selected = false;
            }
            
            newSelectedElement.Selected = true;
            SelectedElement = newSelectedElement;
        }

        public bool HasSelection()
        {
            return SelectedElement != null;
        }

        public void ClearSelection()
        {
            if (!HasSelection())
            {
                return;
            }

            SelectedElement.Selected = false;
            SelectedElement = null;
        }

        public void SetCursor()
        {
            if (HasSelection())
            {
                SelectedElement.BoundingBox.SetCursorPosition();
            }
        }

        public void ClearAllInputs()
        {
            FindAllElementOfType<Input>()
                .FindAll(input => input.ClearOnExit)
                .ForEach(input => input.ClearText());
        }

        public List<SelectableElement> GetAllSelectableElements()
        {
            return CachedSelectableElement.FindAll(element => element.IsSelectable());
        }
        
        public Element FindElement(string name)
        {
            return ElementList.Find(e => e.Name.ToLower().Equals(name.ToLower()));
        }

        public T FindElementOfType<T>(string name) where T : Element
        {
            return (T) ElementList.Find(e => e is T && e.Name.ToLower().Equals(name.ToLower()));
        }

        public List<T> FindAllElementOfType<T>() where T : Element
        {
            return ElementList.FindAll(element => element is T).ConvertAll(element => (T) element);
        }

        public void AddToUpdateQueue(Element element)
        {
            UpdateQueue.Add(element);
        }
    }
}