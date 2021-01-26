using System;
using System.Collections.Generic;
using System.Linq;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class Screen
    {
        public ConsoleManager Manager { get; set; }
        public string Name { get; set; }
        public List<Element> ElementList { get; set; }
        public List<SelectableElement> CachedSelectableElement { get; set; }
        public int SelectedIndex { get; set; }
        public SelectableElement SelectedElement { get; set; }
        public List<Element> UpdateQueue { get; set; }
        public bool Active { get; set; }

        public Screen(ConsoleManager manager)
        {
            Manager = manager;
            ElementList = new List<Element>();
            CachedSelectableElement = new List<SelectableElement>();
            UpdateQueue = new List<Element>();
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

        public virtual void Load()
        {
            if (Active)
            {
                throw new InvalidOperationException("Nested screen stacking not supported.");
            }

            Display();
            SetSelection(0);
            Active = true;
        }

        public virtual void Unload()
        {
            ClearSelection();
            Active = false;
        }

        public virtual void OnView()
        {
        }

        public virtual void Display()
        {
            CHelper.Clear();
            DisplayElements();
            ColourSelector.Element();
            CHelper.PadRemainingHeight();
            ColourSelector.Default();
            UpdateQueue.Clear();
        }

        public void UpdateDisplay()
        {
            UpdateQueue.ForEach(element => element.Display());
            UpdateQueue.Clear();
            ColourSelector.Default();
        }

        protected virtual void DisplayElements()
        {
            foreach (var element in ElementList.Where(element => !element.Hidden))
            {
                element.UpdateBox();
                element.Display();
            }
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