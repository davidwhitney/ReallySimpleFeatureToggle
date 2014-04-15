using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public static class RenderPartialFeatureExtensions
    {
        public static void RenderPartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName)
        {
            RenderPartialFeature(helper, featureName.ToString(), partialViewName);
        }

        public static void RenderPartialFeature(this HtmlHelper helper, string featureName, string partialViewName)
        {
            WhenEnabled.ForFeature(featureName, helper.ViewData, () => helper.RenderPartial(partialViewName));
        }
        
        public static void RenderPartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName, ViewDataDictionary vdd)
        {
            RenderPartialFeature(helper, featureName.ToString(), partialViewName, vdd);
        }

        public static void RenderPartialFeature(this HtmlHelper helper, string featureName, string partialViewName, ViewDataDictionary vdd)
        {
            WhenEnabled.ForFeature(featureName, vdd, () => helper.RenderPartial(partialViewName, vdd));
        }

        public static void RenderPartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName, object model, ViewDataDictionary vdd)
        {
            RenderPartialFeature(helper, featureName.ToString(), partialViewName, model, vdd);
        }

        public static void RenderPartialFeature(this HtmlHelper helper, string featureName, string partialViewName, object model, ViewDataDictionary vdd)
        {
            WhenEnabled.ForFeature(featureName, vdd, () => helper.RenderPartial(partialViewName, model, vdd));
        }

        public static void RenderPartialFeature(this HtmlHelper helper, Enum featureName, string partialViewName, object model)
        {
            RenderPartialFeature(helper, featureName.ToString(), partialViewName, model);
        }

        public static void RenderPartialFeature(this HtmlHelper helper, string featureName, string partialViewName, object model)
        {
            WhenEnabled.ForFeature(featureName, helper.ViewData, () => helper.RenderPartial(partialViewName, model));
        }
    }
}