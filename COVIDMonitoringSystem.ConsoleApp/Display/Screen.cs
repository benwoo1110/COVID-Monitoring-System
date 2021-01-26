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
        public bool Active { get; set; }

        public Screen(ConsoleManager manager)
        {
            Manager = manager;
            ElementList = new List<Element>();
            CachedSelectableElement = new List<SelectableElement>();
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
        }

        protected virtual void DisplayElements()
        {
            foreach (var element in ElementList.Where(element => !element.Hidden))
            {
                element.UpdateBox();

                if (element.Equals(SelectedElement) && SelectedElement.Enabled)
                {
                    DisplaySelected(element);
                    continue;
                }

                DisplayNormal(element);
            }
        }

        public virtual void DisplayNormal(Element element)
        {
            Console.SetCursorPosition(0, element.BoundingBox.Top);
            ColourSelector.Element();
            element.Display();
            ColourSelector.Default();
        }

        public virtual void DisplaySelected(Element element)
        {
            Console.SetCursorPosition(0, element.BoundingBox.Top);
            ColourSelector.Selected();
            element.Display();
            ColourSelector.Default();
        }

        public void SetSelection(int to)
        {
            if (CachedSelectableElement.Count == 0)
            {
                return;
            }

            if (SelectedElement != null)
            {
                DisplayNormal(SelectedElement);
            }

            SelectedIndex = CoreHelper.Mod(to, CachedSelectableElement.Count);
            SelectedElement = CachedSelectableElement[SelectedIndex];
            DisplaySelected(SelectedElement);
            Console.SetCursorPosition(SelectedElement.BoundingBox.Left, SelectedElement.BoundingBox.Top);
        }

        public void ChangeSelection(int by)
        {
            SetSelection(SelectedIndex + by);
        }

        public void ClearSelection()
        {
            SelectedIndex = -1;
            SelectedElement = null;
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
    }
}