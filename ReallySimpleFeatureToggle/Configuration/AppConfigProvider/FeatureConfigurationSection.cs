using System.ComponentModel;
using System.Configuration;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class FeatureConfigurationSection : ConfigurationSection, IFeatureConfigurationSection
    {
        private static readonly ConfigurationProperty ConfigurationProperties =
            new ConfigurationProperty(null, typeof (FeatureConfigurationElementCollection), null,
                                      ConfigurationPropertyOptions.IsDefaultCollection);

        [TypeConverter(typeof (CommaDelimitedStringCollectionConverter))]
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public FeatureConfigurationElementCollection FeatureSettings
        {
            get { return ((FeatureConfigurationElementCollection)this[ConfigurationProperties]); }
        }
    }
}
