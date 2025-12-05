using System;

namespace TukitaSystem
{
    public class Menu
    {
        private static List<Menu> _extent = new List<Menu>();
        private static readonly string FilePath = "menu.json";
        
        private string _name;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private string _servingTime;
        
        private Dictionary<string, MenuItem> _qualifiedItems = new Dictionary<string, MenuItem>();
        public IReadOnlyDictionary<string, MenuItem> QualifiedItems => _qualifiedItems;
        public bool IsAvailable { get; set; }

        public Menu(string name, TimeSpan startTime, TimeSpan endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            IsAvailable = true;
            
            _extent.Add(this);
        }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Menu name cannot be empty.");
                
                _name = value;
            }
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromDays(1))
                    throw new ArgumentException("Start time must be a valid time of day.");

                _startTime = value;
            }
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromDays(1))
                    throw new ArgumentException("End time must be a valid time of day.");

                if (StartTime != default && value <= StartTime)
                    throw new ArgumentException("End time must be later than start time.");

                _endTime = value;
            }
        }
        
        public string ServingTime
        {
            get
            {
                return $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
            }
        }

        public void AddMenuItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_qualifiedItems.ContainsKey(item.Name))
                throw new InvalidOperationException($"Menu already contains an item with name '{item.Name}'.");

            _qualifiedItems[item.Name] = item;
            item.AddMenu(this);
        }

        public bool GetItem(string name, out MenuItem item)
        {
            return _qualifiedItems.TryGetValue(name, out item);
        }

        public void RemoveMenuItem(MenuItem item)
        {
            if (item == null) return;
            if (_qualifiedItems.ContainsKey(item.Name))
            {
                _qualifiedItems.Remove(item.Name);
                item.RemoveMenu(this);
            }
        }
        
        public static List<Menu> GetExtent()
        {
            return new List<Menu>(_extent);
        }
        
        public static void SaveExtent()
        {
            StorageService.Save(_extent, FilePath);
        }

        public static void LoadExtent()
        {
            _extent = StorageService.Load<Menu>(FilePath);
        }
    }
}