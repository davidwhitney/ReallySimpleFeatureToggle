using System.Configuration;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class DynamicRuleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("rule", IsRequired = true)]
        public string Rule
        {
            get { return (string)this["rule"]; }
            set { this["rule"] = value; }
        }
    }
}