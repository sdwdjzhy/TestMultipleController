using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace ControllerLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ControllerNamespaceConstraintAttribute : Attribute, IActionConstraint
    {
        public int Order => 1;

        /// <inheritdoc />
        public bool Accept(ActionConstraintContext context)
        {
            return ControllerNameSelector.IsValidForRequest?.Invoke(context) ?? true;
        }
 
    }
}