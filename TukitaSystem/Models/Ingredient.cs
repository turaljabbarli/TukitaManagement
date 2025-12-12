
namespace TukitaSystem
{
    public class Ingredient
    {
        private static List<Ingredient> _extend = new List<Ingredient>();

        private string _name;
        private int _currentStock;
        
        private HashSet<MenuItem> _usedInItems = new HashSet<MenuItem>();
        
        public IReadOnlyCollection<MenuItem> UsedInItems => _usedInItems.ToList();

        public bool IsVegetarian { get; set; }
        public bool IsAllergic { get; set; }

        public Ingredient(string name, int currentStock, bool isVegetarian, bool isAllergic)
        {
            Name = name;
            CurrentStock = currentStock;
            IsVegetarian = isVegetarian;
            IsAllergic = isAllergic;

            _extend.Add(this);
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ingredient name cannot be empty.");
                _name = value;
            }
        }

        public int CurrentStock
        {
            get => _currentStock;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stock cannot be negative.");
                _currentStock = value;
            }
        }


        public void AddMenuItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (_usedInItems.Contains(item))
                return;

            _usedInItems.Add(item);

            item.AddIngredient(this);
        }

        public bool RemoveMenuItem(MenuItem item)
        {
            if (item == null || !_usedInItems.Contains(item))
                return false;

            _usedInItems.Remove(item);

            item.RemoveIngredient(this);
            
            return true;
        }

        public static List<Ingredient> GetExtend()
        {
            return new List<Ingredient>(_extend);
        }
    }
}
