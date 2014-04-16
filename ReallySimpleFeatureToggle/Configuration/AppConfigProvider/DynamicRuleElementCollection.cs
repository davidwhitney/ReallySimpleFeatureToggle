using System.Configuration;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class DynamicRuleElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DynamicRuleConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DynamicRuleConfigurationElement)element).Rule;
        }
    }
}