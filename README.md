ReallySimpleFeatureToggle [![Build status](https://ci.appveyor.com/api/projects/status/e4fordvbcynvawej)](https://ci.appveyor.com/project/DavidWhitney/reallysimplefeaturetoggle)
=======================

A feature configuration library for .NET!

With Features as a first class citizen, a pipeline to extend how you evaulate feature availability, multi-tenancy, and other extensibility points, ReallySimpleFeatureToggle should make it easy to do "feature configuration over feature branching".

# Contents

* Getting started - super happy path
* Bootstrapping, containers & lifecycles
* App and Web.config options
* Default Behaviour
* Core Concepts & Pipeline
* Extensibility
* Lineage
* Contributing

# Getting started - super happy path

The happy path use case of this library is trivial - add a configuration section to your app or web config file and start using the library. You don't need to worry about all the extensibility points as we've provided sane defaults for you.


Add the NuGet package

    PM> Install-Package ReallySimpleFeatureToggle

Consider this configuration:
 
      <?xml version="1.0" encoding="utf-8" ?>
      <configuration>
        <configSections>
          <section name="features" type="ReallySimpleFeatureToggle.Configuration.AppConfigProvider.FeatureConfigurationSection, ReallySimpleFeatureToggle" />
        </configSections>

        <features>
          <add name="EnabledFeature" state="Enabled" />
          <add name="DisabledFeature" state="Disabled" />
          <add name="EnabledFor50Percent" state="EnabledForPercentage" randomPercentageEnabled="50" />
        </features>
      </configuration>
      
Now consider this usage example:


        var config = ReallySimpleFeature.Toggles.GetFeatureConfiguration();

        if (config.IsAvailable(FeaturesEnum.EnabledFeature))
        {
            Console.WriteLine("This feature is clearly enabled");
        }

        if (config.IsAvailable(FeaturesEnum.DisabledFeature))
        {
            Console.WriteLine("You'll never see this.");
        }

        const int maxTries = 50000;
        var wasTrue = 0;
        for (var i = 0; i != maxTries; i++)
        {
            var recalculatedConfiguration = ReallySimpleFeature.Toggles.GetFeatureConfiguration();
            if (recalculatedConfiguration.IsAvailable(FeaturesEnum.EnabledFor50Percent))
            {
                wasTrue++;
            }
        }

        Console.WriteLine("Enabled for 50% was enabled: " + wasTrue + " times out of " + maxTries + " - Approx Percent: " + (100 * (maxTries - wasTrue) / maxTries));

That's really it. You can store your feature names in either an enum or a string const like this:

        public enum FeaturesEnum
        {
            EnabledFeature,
            DisabledFeature,
            EnabledFor50Percent
        }

        public class FeaturesStrings
        {
            public const string EnabledFeature = "EnabledFeature";
            public const string DisabledFeature = "DisabledFeature";
            public const string EnabledFor50Percent = "EnabledFor50Percent";
        }        

# Bootstrapping, containers & lifecycles

Bind `FeatureConfiguration` to `ReallySimpleFeature.Toggles.GetFeatureConfiguration();` in your container In Request Scope / In unit of work scope.

OR

Call `ReallySimpleFeature.Toggles.GetFeatureConfiguration();` each time you wish your feature state and associated rules and overrides to be executed

# App and Web.config options

By default, you can configure your features using the following options:

      <features>
        <add name="EnabledFeature" state="Enabled" />
        <add name="DisabledFeature" state="Disabled" />
        <add name="SupportedAllFeature" state="Enabled" supportedTenants="All" />
        <add name="SupportedStringEmpty" state="Enabled" supportedTenants="" />
        
        <add name="FeatureWithDependency" state="Enabled" dependencies="FeatureWithDependant" />
        <add name="FeatureWithDependant" state="Enabled" />
        
        <add name="FeatureWithDisabledDependency" state="Enabled" dependencies="DisabledFeatureWithDependant" />
        <add name="DisabledFeatureWithDependant" state="Disabled" />
        
        <add name="FeatureForMultipleTenants" state="Enabled" supportedTenants="Tenant1,Tenant2,Tenant3" />
        <add name="EnabledFor50Percent" state="EnabledForPercentage" randomPercentageEnabled="50" />
        
      </features>
      
You can configure features to be available:

  * By state alone ("Enabled", "Disabled")
  * For a specific tenant or set of tenants
  * For everyone *except* an excluded tenant
  * To be dependant on another feature
  * To be enabled for a percentage
  
Or any combination of the above.


# Default Behaviour

* Every time you call `ReallySimpleFeature.Toggles.GetFeatureConfiguration();` your features state is evaluated. This is the method you want to bind into your IoC container / unit of work.
* Your feature settings are cached in memory for 30 seconds - so if you're sourcing the settings from a database provider, there will be a small lag between you changing settings and them being read.
* If you attempt to call `IsAvailable("xyz)` on a feature that doesn't exist in the feature settings, an exception will be thrown.
* `ReallySimpleFeature.Toggles` is designed to be a singleton for the lifetime of your application, it stores the configuration for the feature toggling library, though if you want to create your own instance of `ReallySimpleFeature` you're welcome to.
* Multi-tenancy is supported by default by calling `ReallySimpleFeature.Toggles.GetFeatureConfiguration("TenantName");`
* Features can depend on other features.


# Core Concepts & Pipeline

The evaluation of feature states runs through the following pipeline:

## Load Feature Settings 

This can be stored anywhere, but by default, we provide an App/Web.config handler to store your feature settings with your app. There's also code based FluentAPI provided if you'd rather define your features at compile time (though with less runtime flexibility).

## Apply IAvailabilityRule's

Pluggable availability rules that define how your feature settings are evaulated. By default, we ship with availability rules that check if your feature is "on", "off" or "enabled for a percentage of users", along with ensuring any date requirements ("feature available between these two dates") and dependencies ("features that depend on other features") are satisfied. **All configured feature availability rules must return true for a feature to be considered enabled**.

## Apply IFeatureOverrideRule's

Once all the availability rules have been executed over your feature settings, any configured feature overrides are executed. Feature overrides allow you to short circuit the state of a feature, forcing its state to enabled or disabled. An example of this is an override provided (but not configured by default) in the ReallySimpleFeatureToggle.Web.dll that will force any features on if their names are detected in the HttpContext.Current.Request.Url.QueryString - great for debugging and QA.

## Return computed config

The feature configuration is the computed final collection of features and their states. You can ask the FeatureConfiguration if your feature is enabled by calling `cfg.IsAvailable("MyFeature")` and the result will take into account the availability rules and overrides you have configured.


# Extensibility

There's a Fluent API available to help configure some of the more advanced portions of the feature library. Here's a complete example:

      var featureSet = new ReallySimpleFeature().Configure
                .GlobalAvailabilityRules(x =>
                {
                    x.Add(new TestAvailabilityRuleThatReturns(false));
                })
                .GlobalOverrides(x =>
                {
                    x.Add(new TestOverrideThatDoesNothing());
                })
                .WhenFeatureRequestedIsNotConfigured(new FeatureShouldBeEnabled())
                .WithFeatureConfigurationSource(new AppConfigFeatureRepository(), new TimeSpan(500))
                .And().GetFeatureConfiguration();
                
This API offers the following:

* Modify Global availability rules - these are the rules must all return true for your feature to be "Enabled" - it's unlikely you'll end up modifying these
* Register IFeatureOverrides - these rules execute after feature evaluation and will let you plumb in any debug hooks or overloads
* Control the behaviour of the library when no feature settings are found, defaulting to `ThrowANotConfiguredException` (other options are: `FeatureShouldBeEnabled`, `FeatureShouldBeDisabled`)
* Wire up an `IFeatureRepository` - the component used to read the initial feature settings (the interface is: `ICollection<IFeature> GetFeatureSettings();`) and optionally tweak the feature setting cache duration, or disable it entirely.

## Feature scoped Availability Rules

// todo: write docs

## Dynamic Availability Rules

// todo: write docs

## Lambda Availability Rules

// todo: write docs

## Plugins - IPluginBootstrapper

// todo: write docs

* ReallySimpleFeatureToggle.Web
* ReallySimpleFeatureToggle.Web.Mvc


# Lineage

This library is the "third cousin" of NFeature (https://github.com/benaston/NFeature) - originally derived from the same initial prototype - as such there might be some code in common.


The code-bases are long since diverged and now barely related, but you might notice similar concepts or lines of code (especially around the AppConfig handling code), as a result, this library is also licensed under the GNU Lesser GPL with this notice.


If you don't like my implementation, go check out Ben's, It's got a different set of features and functionality.

# Contributing

* Raise a GitHub Issue
* Engage in discussion
* Write a test
* Fix a bug!
* Send a PR!

Steer clear of big refactors without discussion first, don't diverge from the coding conventions in the project, y'all know the drill. All contributions welcome!
