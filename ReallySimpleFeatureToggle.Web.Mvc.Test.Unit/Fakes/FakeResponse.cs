using System.Web;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.Fakes
{
    public class FakeResponse : HttpResponseBase
    {
        public override int StatusCode { get; set; }
    }
}