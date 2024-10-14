using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.Tests.ExampleObjects
{
    public class Event
    {
        public string Name { get; set; }

        public string LocationName { get; set; }

        public DateTime Time { get; set; }

        public ExtraInfo AdditionalInfo { get; set; }
    }

    public class ExtraInfo
    {
        public string Example1 { get; set; }

        public int Example2 { get; set; }
    }
}
