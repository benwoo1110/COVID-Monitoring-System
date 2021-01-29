﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var methodsMap = ReflectHelper.GetMethodWithAttribute<OnClick>(this);
            foreach (var (method, clickAttr) in methodsMap)
            {
                var button = FindElementOfType<Button>(clickAttr.ButtonName);
                if (button == null)
                {
                    throw new InvalidOperationException($"Button {clickAttr.ButtonName} not found.");
                }

                button.MethodRunner = new ButtonMethod(method);
            }

            //TODO: Make this better
            var methodsMap2 = ReflectHelper.GetMethodWithAttribute<OnEnterInput>(this);
            foreach (var (action, clickAttr) in methodsMap2)
            {
                var input = FindElementOfType<Input>(clickAttr.InputName);
                if (input == null)
                {
                    throw new InvalidOperationException($"Button {clickAttr.InputName} not found.");
                }
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
            ColourSelector.Element();
        }

        public void Update()
        {
            var queueSnapshot = new List<Element>(UpdateQueue);
            UpdateQueue.Clear();
            
            queueSnapshot.ForEach(element => element.Render());
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
            var targetElement = CachedSelectableElement[SelectedIndex];

            if (targetElement.Hidden || !targetElement.Enabled)
            {
                return;
            }

            SelectedElement = targetElement;
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