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
                var value = this["state"];
                if (value is State)
                {
                    return (State)value;
                }

                return (State)Enum.Parse(typeof(State), value.ToString(), true);
            }
            set
            {
                this["state"] = value;
            }
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
                var collection = this["supportedTenants"] as CommaDelimitedStringCollection;
                if (collection != null)
                {
                    return (collection.Cast<string>().Select(t => t)).ToArray();
                }

                if (this["supportedTenants"] is string[])
                {
                    return this["supportedTenants"] as string[];
                }

                return new [] {Tenant.All};
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
                if (collection != null)
                {
                    return collection.Cast<string>().ToArray();
                }

                if (this["excludedTenants"] is string[])
                {
                    return this["excludedTenants"] as string[];
                }

                return new string[0];
            }
            set { this["excludedTenants"] = value; }
        }

        [TypeConverter(typeof (CommaDelimitedStringCollectionConverter))]
        [ConfigurationProperty("dependencies", IsRequired = false)]
        public string[] Dependencies
        {
            get
            {
                if (!(this["dependencies"] is CommaDelimitedStringCollection))
                {
                    return this["dependencies"] as string[];
                }

                var csv = (this["dependencies"] as CommaDelimitedStringCollection);
                return (csv.Cast<string>().Select(t => t)).ToArray();
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
            set { this["randomPercentageEnabled"] = value; }
        }
    }
}