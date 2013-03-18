using System.Configuration;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class FeatureConfigurationElementCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public FeatureConfigurationElement this[int index]
        {
            get { return (FeatureConfigurationElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(FeatureConfigurationElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FeatureConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FeatureConfigurationElement)element).Name;
        }

        public void Remove(FeatureConfigurationElement element)
        {
            BaseRemove(element.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }
}