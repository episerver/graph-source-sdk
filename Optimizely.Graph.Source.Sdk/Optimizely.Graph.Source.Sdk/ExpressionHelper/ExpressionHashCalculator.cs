using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Optimizely.Graph.Source.Sdk.ExpressionHelper
{
    public class ExpressionHashCalculator
    {
        private static PropertyInfo _debugViewProperty = typeof(Expression).GetProperty("DebugView", BindingFlags.Instance | BindingFlags.NonPublic);

        public string CalculateHashCode(Expression obj)
        {
            return _debugViewProperty.GetValue(obj) as string;
        }
    }
}
