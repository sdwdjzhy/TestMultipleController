using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace TestMultipleController
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ControllerNamespaceConstraintAttribute : Attribute, IActionConstraint
    {
        public int Order => 1;
        /// <inheritdoc />
        public bool Accept(ActionConstraintContext context)
        {
            return IsValidForRequest(context.RouteContext, context.CurrentCandidate.Action);
        }
        public bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var actionNamespace = ((ControllerActionDescriptor)action).MethodInfo.DeclaringType.Namespace;
            Console.WriteLine("IsValidForRequest:" + actionNamespace);
            Console.WriteLine("routeContext.RouteData.DataTokens:" + routeContext.RouteData.DataTokens.Count());
            if (routeContext.RouteData.DataTokens.ContainsKey("Namespace"))
            {
                var dataTokenNamespace = (string)routeContext.RouteData.DataTokens.FirstOrDefault(dt => dt.Key == "Namespace").Value;
                return dataTokenNamespace == actionNamespace;
            }
            return true;
        }
    }
}
