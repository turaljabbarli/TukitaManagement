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

        // --- QUALIFIED ASSOCIATION: Dictionary<Qualifier, Object> ---
        // Qualifier: MenuItem Name (string)
        // Value: MenuItem object
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

        // Expose as ReadOnlyDictionary to protect the internal structure
        public IReadOnlyDictionary<string, MenuItem> QualifiedItems => new ReadOnlyDictionary<string, MenuItem>(_items);

        // --- STRICT RECURSION GUARD (Qualified) ---

        public void AddMenuItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // 1. Guard: Check by Key (Qualifier)
            if (_items.ContainsKey(item.Name))
            {
                return;
            }

            // 2. Add Locally (Key, Value)
            _items.Add(item.Name, item);

            // 3. Trigger Reverse Connection
            item.AddMenu(this);
        }

        public void RemoveMenuItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // 1. Guard
            if (!_items.ContainsKey(item.Name))
            {
                return;
            }

            // 2. Remove Locally
            _items.Remove(item.Name);

            // 3. Trigger Reverse Connection
            item.RemoveMenu(this);
        }

        public static List<Menu> GetExtent() => new List<Menu>(_extent);
        public static void SaveExtent() => StorageService.Save(_extent, FilePath);
        public static void LoadExtent() => _extent = StorageService.Load<Menu>(FilePath);
    }
}