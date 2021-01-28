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
            elementList.ForEach(AddElement);
        }

        private void AddButtonMethods()
        {
            var methodsMap = ReflectHelper.GetMethodWithAttribute<Action, OnClick>(this);
            foreach (var (action, clickAttr) in methodsMap)
            {
                var button = FindElementOfType<Button>(clickAttr.ButtonName);
                if (button == null)
                {
                    throw new InvalidOperationException($"Button {clickAttr.ButtonName} not found.");
                }

                button.Runner = action;
            }

            //TODO: Make this better
            var methodsMap2 = ReflectHelper.GetMethodWithAttribute<Action, OnEnterInput>(this);
            foreach (var (action, clickAttr) in methodsMap2)
            {
                var input = FindElementOfType<Input>(clickAttr.InputName);
                if (input == null)
                {
                    throw new InvalidOperationException($"Button {clickAttr.InputName} not found.");
                }

                input.OnEnterRunner = action;
            }
        }
        
        public void AddElement(Element element)
        {
            element.TargetAbstractScreen = this;
            ElementList.Add(element);

            if (element is SelectableElement selectableElement)
            {
                CachedSelectableElement.Add(selectableElement);
            }
        }

        public virtual void Load()
        {
            if (Active)
            {
                throw new InvalidOperationException("Nested screen stacking not supported.");
            }

            Active = true;
            Render();
            SetSelection(0);
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
        }

        public virtual void Render()
        {
            CHelper.Clear();
            foreach (var element in ElementList.Where(element => !element.Hidden))
            {
                element.Render();
            }
            ColourSelector.Element();
            UpdateQueue.Clear();
        }

        public void Update()
        {
            UpdateQueue.ForEach(element => element.Render());
            UpdateQueue.Clear();
            ColourSelector.Element();
        }

        public void SetSelection(int to)
        {
            if (CachedSelectableElement.Count == 0)
            {
                return;
            }

            if (SelectedElement != null)
            {
                SelectedElement.Selected = false;
            }

            SelectedIndex = CoreHelper.Mod(to, CachedSelectableElement.Count);
            SelectedElement = CachedSelectableElement[SelectedIndex];
            SelectedElement.Selected = true;
        }

        public void ChangeSelection(int by)
        {
            SetSelection(SelectedIndex + by);
        }

        public void ClearSelection()
        {
            if (!HasSelection())
            {
                return;
            }

            SelectedElement.Selected = false;
            SelectedElement = null;
            SelectedIndex = -1;
        }

        public bool HasSelection()
        {
            return SelectedIndex >= 0 && SelectedElement != null;
        }

        public Element FindElement(string name)
        {
            return ElementList.Find(e => e.Name.ToLower().Equals(name.ToLower()));
        }

        public T FindElementOfType<T>(string name) where T : Element
        {
            return (T) ElementList.Find(e => e is T && e.Name.ToLower().Equals(name.ToLower()));
        }

        public void AddToUpdateQueue(Element element)
        {
            UpdateQueue.Add(element);
        }
    }
}