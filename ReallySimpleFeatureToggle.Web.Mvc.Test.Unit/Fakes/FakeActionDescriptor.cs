using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.Fakes
{
    public class FakeActionDescriptor : ActionDescriptor
    {
        public string _actionName;

        public override string ActionName
        {
            get { return _actionName; }
        }

        public override ControllerDescriptor ControllerDescriptor { get; }

        public FakeActionDescriptor(Type type)
        {
            ControllerDescriptor = new FakeControllerDescriptor(type);
        }

        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override ParameterDescriptor[] GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}