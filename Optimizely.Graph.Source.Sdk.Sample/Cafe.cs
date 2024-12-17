using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.Sample
{
    public class Cafe
    {
        public string Name { get; set; }

        public DateTime Established { get; set; }

        public Location Address { get; set; }

        public Menu Menu { get; set; }

        public override string ToString()
        {
            return $"{Name}_{1}";
        }
    }

    public class Location
    {
        public string City { get; set; }

        public string State { get; set; }

        public string Zipcode { get; set; }

        public string Country { get; set; }  
    }

    public class Menu
    {
        public IEnumerable<Beverage> Beverages { get; set; }

        public IEnumerable<FoodItem> Food { get; set; }
    }

    public class Beverage
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public IEnumerable<string> Sizes { get; set; }

    }

    public class FoodItem
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public bool IsAvaiable { get; set; }
    }
}
