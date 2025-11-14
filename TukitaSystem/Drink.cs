using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Drink : MenuItem
    {
        public Drink(string name, decimal price, int calories, int preparationTime, bool isCarbonated, Size size)
            : base(name, price, calories, preparationTime)
        {
            IsCarbonated = isCarbonated;
            Size = size;
        }

        public bool IsCarbonated { get; set; }
        public Size Size { get; set; }
    }
}