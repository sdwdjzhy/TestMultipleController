using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TestMultipleControllerWeb
{
    public class ActionModelMatchingEqualityComparer : IEqualityComparer<ActionModel>
    {
        static readonly Lazy<ActionModelMatchingEqualityComparer> _instanceLazy =
            new Lazy<ActionModelMatchingEqualityComparer>(() => new ActionModelMatchingEqualityComparer());
        public static ActionModelMatchingEqualityComparer Instance => _instanceLazy.Value;
        public bool Equals(ActionModel x, ActionModel y)
        {
            return string.Equals(x.ActionName, y.ActionName, StringComparison.OrdinalIgnoreCase) &&
                   x.Attributes.OfType<IActionHttpMethodProvider>().SelectMany(e => e.HttpMethods).DefaultIfEmpty("GET")
                    .Intersect(y.Attributes.OfType<IActionHttpMethodProvider>().SelectMany(e => e.HttpMethods).DefaultIfEmpty("GET"),
                               StringComparer.OrdinalIgnoreCase).Any();

        }

        public int GetHashCode(ActionModel obj)
        {
            return (obj.ActionName?.GetHashCode() ?? 0) ^
                    obj.Attributes.OfType<IActionHttpMethodProvider>()
                       .SelectMany(e => e.HttpMethods).Distinct(StringComparer.OrdinalIgnoreCase)
                       .OrderBy(e => e).Aggregate(0, (c, e) => (c * 13) ^ e.GetHashCode());
        }
    }
}
