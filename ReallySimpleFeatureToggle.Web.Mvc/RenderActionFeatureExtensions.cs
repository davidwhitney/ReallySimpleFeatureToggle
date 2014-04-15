using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public static class RenderActionFeatureExtensions
    {
        public static void ActionFeature(this HtmlHelper helper, Enum featureName, string actionName)
        {
            ActionFeature(helper, featureName.ToString(), actionName);
        }

        public static void ActionFeature(this HtmlHelper helper, string featureName, string actionName)
        {
            WhenEnabled.ForFeature(featureName, helper.ViewData, () => helper.RenderAction(actionName));
        }

        public static void ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, ViewDataDictionary routeValues)
        {
            ActionFeature(helper, featureName.ToString(), actionName, routeValues);
        }

        public static void ActionFeature(this HtmlHelper helper, string featureName, string actionName, ViewDataDictionary routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () => helper.RenderAction(actionName, routeValues));
        }

        public static void ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, object routeValues)
        {
            ActionFeature(helper, featureName.ToString(), actionName, routeValues);
        }

        public static void ActionFeature(this HtmlHelper helper, string featureName, string actionName, object routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () => helper.RenderAction(actionName, routeValues));
        }

        public static void ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, string controllerName, ViewDataDictionary routeValues)
        {
            ActionFeature(helper, featureName.ToString(), actionName, controllerName, routeValues);
        }

        public static void ActionFeature(this HtmlHelper helper, string featureName, string actionName, string controllerName, ViewDataDictionary routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () => helper.RenderAction(actionName, controllerName, routeValues));
        }

        public static void ActionFeature(this HtmlHelper helper, Enum featureName, string actionName, string controllerName, object routeValues)
        {
            ActionFeature(helper, featureName.ToString(), actionName, controllerName, routeValues);
        }

        public static void ActionFeature(this HtmlHelper helper, string featureName, string actionName, string controllerName, object routeValues)
        {
            WhenEnabled.ForFeature(featureName, routeValues, () => helper.RenderAction(actionName, controllerName, routeValues));
        }
    }
}