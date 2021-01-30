using System;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace ControllerLib
{
    public static class ControllerNameSelector
    {
        /// <summary>
        /// 是否可以调用此Controller，Action
        /// </summary>
        public static Func<ActionConstraintContext, bool> IsValidForRequest;
    }
}