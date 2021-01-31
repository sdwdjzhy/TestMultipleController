using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace TestMultipleControllerWeb
{
    public class DuplicateActionsRemovalApplicationModelConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            Console.WriteLine("DuplicateActionsRemovalApplicationModelConvention");
            var currentExecutingAsm = Assembly.GetExecutingAssembly();
            var allControllers = application.Controllers;

            var overriddenActions = allControllers.GroupBy(e => e.ControllerName)
                                               //select only duplicate controllers
                                               .Where(g => g.Count() > 1)
                                               .SelectMany(g => g.SelectMany(e => e.Actions)
                                                                 .GroupBy(o => o, ActionModelMatchingEqualityComparer.Instance)
                                                                 //select only duplicate actions
                                                                 .Where(k => k.Count() > 1)
                                                                 .SelectMany(e => e.OrderByDescending(x => x.Controller.ControllerType.Assembly == currentExecutingAsm)
                                                                     //select all except the action defined in the current executing assembly
                                                                     .Skip(1)))
                                               .Select(e => (controller: e.Controller, action: e));
            //for each overridden action, just remove it from the owner controller
            //this effectively means that the removed action will be overridden
            //by the only remaining one (defined in the current executing assembly)
            foreach (var overriddenAction in overriddenActions)
            {
                overriddenAction.controller.Actions.Remove(overriddenAction.action);
            }
        }
    }
}
