using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public static class RenderActionFeatureExtensions
    {
        public static void RenderActionFeature(this HtmlHelper helper, Enum featureName, string actionName)
        {
            RenderActionFeature(helper, featureName.ToString(), actionName);
        }

        public static void RenderActionFeature(this HtmlHelper helper, string featureName, string actionName)
        {
            WhenEnabled.ForFeature(featureName, helper.ViewData, () => helper.RenderAction(actionName, new { FeatureName = featureName }));
        }

        public static void RenderActionFeature(this HtmlHelper helper, Enum featureName, string actionName, ViewDataDictionary routeValues)
        {
            RenderActionFeature(helper, featureName.ToString(), actionName, routeValues);
        }

        public static void RenderActionFeature(this HtmlHelper helper, string featureName, string actionName, ViewDataDictionary routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                routeValues.Add(WhenEnabled.FeatureNameKey, featureName);
                helper.RenderAction(actionName, routeValues);
            });
        }

        public static void RenderActionFeature(this HtmlHelper helper, Enum featureName, string actionName, object routeValues)
        {
            RenderActionFeature(helper, featureName.ToString(), actionName, routeValues);
        }

        public static void RenderActionFeature(this HtmlHelper helper, string featureName, string actionName, object routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                var rvd = new RouteValueDictionary(routeValues) { { WhenEnabled.FeatureNameKey, featureName } };
                helper.RenderAction(actionName, rvd);
            });
        }

        public static void RenderActionFeature(this HtmlHelper helper, Enum featureName, string actionName, string controllerName, ViewDataDictionary routeValues)
        {
            RenderActionFeature(helper, featureName.ToString(), actionName, controllerName, routeValues);
        }

        public static void RenderActionFeature(this HtmlHelper helper, string featureName, string actionName, string controllerName, ViewDataDictionary routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                routeValues.Add(WhenEnabled.FeatureNameKey, featureName);
                helper.RenderAction(actionName, controllerName, routeValues);
            });
        }

        public static void RenderActionFeature(this HtmlHelper helper, Enum featureName, string actionName, string controllerName, object routeValues)
        {
            RenderActionFeature(helper, featureName.ToString(), actionName, controllerName, routeValues);
        }

        public static void RenderActionFeature(this HtmlHelper helper, string featureName, string actionName, string controllerName, object routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                var rvd = new RouteValueDictionary(routeValues) { { WhenEnabled.FeatureNameKey, featureName } };
                helper.RenderAction(actionName, controllerName, rvd);
            });
        }
    }
}