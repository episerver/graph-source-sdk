using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.Sample
{
    public class ExampleData
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"{FirstName}_{LastName}_{Age}";
        }
    }
}
