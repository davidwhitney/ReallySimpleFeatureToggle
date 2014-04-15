using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public static class PartialFeatureExtensions
    {
        public static MvcHtmlString PartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName)
        {
            return PartialFeature(helper, featureName.ToString(), partialViewName);
        }

        public static MvcHtmlString PartialFeature(this HtmlHelper helper, string featureName, string partialViewName)
        {
            return WhenEnabled.ForFeature(featureName, helper.ViewData, () => helper.Partial(partialViewName));
        }

        public static MvcHtmlString PartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName, ViewDataDictionary vdd)
        {
            return PartialFeature(helper, featureName.ToString(), partialViewName, vdd);
        }

        public static MvcHtmlString PartialFeature(this HtmlHelper helper, string featureName, string partialViewName, ViewDataDictionary vdd)
        {
            return WhenEnabled.ForFeature(featureName, vdd, () => helper.Partial(partialViewName, vdd));
        }

        public static MvcHtmlString PartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName, object model, ViewDataDictionary vdd)
        {
            return PartialFeature(helper, featureName.ToString(), partialViewName, model, vdd);
        }

        public static MvcHtmlString PartialFeature(this HtmlHelper helper, string featureName, string partialViewName, object model, ViewDataDictionary vdd)
        {
            return WhenEnabled.ForFeature(featureName, vdd, () => helper.Partial(partialViewName, model, vdd));
        }

        public static MvcHtmlString PartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName, object model)
        {
            return PartialFeature(helper, featureName.ToString(), partialViewName, model);
        }

        public static MvcHtmlString PartialFeature(this HtmlHelper helper, string featureName, string partialViewName, object model)
        {
            return WhenEnabled.ForFeature(featureName, helper.ViewData, () => helper.Partial(partialViewName, model));
        }
    }
}