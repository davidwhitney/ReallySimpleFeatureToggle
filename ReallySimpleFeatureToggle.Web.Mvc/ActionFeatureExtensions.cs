using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public static class ActionFeatureExtensions
    {
        public static MvcHtmlString ActionFeature(this HtmlHelper helper, Enum featureName, string actionName)
        {
            return ActionFeature(helper, featureName.ToString(), actionName);
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, string featureName, string actionName)
        {
            return WhenEnabled.ForFeature(featureName, helper.ViewData, () => helper.Action(actionName, new { FeatureName = featureName }));
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, ViewDataDictionary routeValues)
        {
            return ActionFeature(helper, featureName.ToString(), actionName, routeValues);
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, string featureName, string actionName, ViewDataDictionary routeValues)
        {
            return WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                routeValues.Add(WhenEnabled.FeatureNameKey, featureName);
                return helper.Action(actionName, routeValues);
            });
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, object routeValues)
        {
            return ActionFeature(helper, featureName.ToString(), actionName, routeValues);
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, string featureName, string actionName, object routeValues)
        {
            return WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                var rvd = new RouteValueDictionary(routeValues) { { WhenEnabled.FeatureNameKey, featureName } };
                return helper.Action(actionName, rvd);
            });
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, string controllerName, ViewDataDictionary routeValues)
        {
            return ActionFeature(helper, featureName.ToString(), actionName, controllerName, routeValues);
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, string featureName, string actionName, string controllerName, ViewDataDictionary routeValues)
        {
            return WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                routeValues.Add(WhenEnabled.FeatureNameKey, featureName);
                return helper.Action(actionName, controllerName, routeValues);
            });
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, string controllerName, object routeValues)
        {
            return ActionFeature(helper, featureName.ToString(), actionName, controllerName, routeValues);
        }

        public static MvcHtmlString ActionFeature(this HtmlHelper helper, string featureName, string actionName, string controllerName, object routeValues)
        {
            return WhenEnabled.ForFeature(featureName, routeValues, () =>
            {
                var rvd = new RouteValueDictionary(routeValues) { { WhenEnabled.FeatureNameKey, featureName } };
                return helper.Action(actionName, controllerName, rvd);
            });
        }
    }
}