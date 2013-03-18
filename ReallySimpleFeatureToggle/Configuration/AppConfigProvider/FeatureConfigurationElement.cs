using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    /// <example>
    ///   <![CDATA[
    /// <add name="FeatureName" state="Enabled" supportedTenants="SomeTenant,CanBeLeftBlank,All" dependencies="MyFeature" />    
    /// ]]>
    /// </example>
    public class FeatureConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Defaults to Enabled
        /// </summary>
        [ConfigurationProperty("state", IsRequired = false)]
        public State State
        {
            get
            {           
                return (int)this["state"] == 0 ? State.Enabled : (State)this["state"];
            }
            set { this["state"] = Enum.GetName(typeof (State), value); }
        }

        /// <summary>
        /// Defaults to 'All'.
        /// </summary>
        [TypeConverter(typeof (CommaDelimitedStringCollectionConverter))]
        [ConfigurationProperty("supportedTenants", IsRequired = false)]
        public string[] SupportedTenants
        {
            get
            {
                if (this["supportedTenants"] is CommaDelimitedStringCollection)
                {
                    return (((CommaDelimitedStringCollection)this["supportedTenants"]).Cast<string>().Select(t => t)).ToArray();
                }

                var tenants = this["supportedTenants"] as string[];
                return tenants ?? new [] {Tenant.All};
            }
            set { this["supportedTenants"] = value; }
        }

        [TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
        [ConfigurationProperty("excludedTenants", IsRequired = false)]
        public string[] ExcludedTenants
        {
            get
            {
                var collection = this["excludedTenants"] as CommaDelimitedStringCollection;
                return collection == null
                           ? new string[0]
                           : collection.Cast<string>().ToArray();
            }
            set { this["excludedTenants"] = value; }
        }

        [TypeConverter(typeof (CommaDelimitedStringCollectionConverter))]
        [ConfigurationProperty("dependencies", IsRequired = false)]
        public string[] Dependencies
        {
            get
            {
                var dependencies = ((CommaDelimitedStringCollection) this["dependencies"])
                                   ?? new CommaDelimitedStringCollection();

                return (dependencies.Cast<string>().Select(t => t)).ToArray();
            }
            set { this["dependencies"] = value; }
        }

        [TypeConverter(typeof (StringToEnGbDateTimeConverter))]
        [ConfigurationProperty("startDtg", IsRequired = false)]
        public DateTime StartDtg
        {
            get { return (DateTime)this["startDtg"]; }
            set { this["startDtg"] = value; }
        }
        
        [TypeConverter(typeof (StringToEnGbDateTimeConverter))]
        [ConfigurationProperty("endDtg", IsRequired = false)]
        public DateTime EndDtg
        {
            get
            {
                return (DateTime)this["endDtg"];                               
            }
            set { this["endDtg"] = value; }
        }

        [ConfigurationProperty("randomPercentageEnabled", IsRequired = false)]
        public int RandomPercentageEnabled
        {
            get { return (int) this["randomPercentageEnabled"]; }
        }
    }
}