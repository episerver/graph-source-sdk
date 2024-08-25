using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.Sample
{
    public class PersonDetails
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public BirthDate BirthDate { get; set; } = new BirthDate();

        public Location Location { get; set; } = new Location();

        public override string ToString()
        {
            return $"{FirstName}_{LastName}_{Age}";
        }

    }

    public class BirthDate
    {
        public int Month { get; set; }

        public int Day { get; set; }

        public int Year { get; set; }
    }

    public class Location
    {
        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Zip { get; set; }
    }
}
