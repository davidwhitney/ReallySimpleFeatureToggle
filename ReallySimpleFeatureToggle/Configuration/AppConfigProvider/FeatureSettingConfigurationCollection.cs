using System.Configuration;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class FeatureConfigurationElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }
        
        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureConfigurationElement)element).Name;
        }
    }
}