using System;
using System.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.Fakes
{
    public class FakeControllerDescriptor: ControllerDescriptor
    {
        public override Type ControllerType { get; }

        public override string ControllerName { get; }

        public FakeControllerDescriptor(Type type)
        {
            ControllerType = type;
            ControllerName = type.Name;
        }

        public override ActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
        {
            throw new NotImplementedException();
        }

        public override ActionDescriptor[] GetCanonicalActions()
        {
            throw new NotImplementedException();
        }
    }
}