using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TukitaSystem
{
    public class Menu
    {
        private static List<Menu> _extent = new List<Menu>();
        private static readonly string FilePath = "menus.json";

        private string _name;
        private string _servingTime;
        public bool IsAvailable { get; set; }

        private Dictionary<string, MenuItem> _items = new Dictionary<string, MenuItem>();

        public Menu(string name, string servingTime)
        {
            Name = name;
            ServingTime = servingTime;
            IsAvailable = true;

            _extent.Add(this);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Menu name cannot be empty.");
                _name = value;
            }
        }

        public string ServingTime
        {
            get => _servingTime;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Serving time cannot be empty.");
                _servingTime = value;
            }
        }

        public IReadOnlyDictionary<string, MenuItem> QualifiedItems => new ReadOnlyDictionary<string, MenuItem>(_items);


        public void AddMenuItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (_items.ContainsKey(item.Name))
            {
                return;
            }

            _items.Add(item.Name, item);

            item.AddMenu(this);
        }

        public void RemoveMenuItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (!_items.ContainsKey(item.Name))
            {
                return;
            }

            _items.Remove(item.Name);

            item.RemoveMenu(this);
        }

        public static List<Menu> GetExtent() => new List<Menu>(_extent);
        public static void SaveExtent() => StorageService.Save(_extent, FilePath);
        public static void LoadExtent() => _extent = StorageService.Load<Menu>(FilePath);
    }
}
