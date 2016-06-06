using System.Web;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.Fakes
{
    public class FakeHttpContext : HttpContextBase
    {
        public override HttpRequestBase Request { get; }
        public override HttpResponseBase Response { get; }

        public FakeHttpContext()
        {
            Request = new FakeRequest();
            Response = new FakeResponse();
        }
    }
}