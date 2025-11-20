using System;
using System.Collections.Generic;

namespace TukitaSystem
{
    public class Drink : MenuItem
    {
        public Drink(string name, decimal price, int calories, int preparationTime, bool isCarbonated, SizeType sizeType)
            : base(name, price, calories, preparationTime)
        {
            IsCarbonated = isCarbonated;
            SizeType = sizeType;
        }

        public bool IsCarbonated { get; set; }
        public SizeType SizeType { get; set; }
    }
}