using System;

namespace ReallySimpleFeatureToggle.ExampleApplicationUsage
{

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

    class Program
    {
        static void Main(string[] args)
        {
            var config = ReallySimpleFeature.Toggles.GetFeatureConfiguration();

            if (config.IsAvailable(FeaturesEnum.EnabledFeature))
            {
                Console.WriteLine("This feature is clearly enabled");
            }
            else
            {
                Console.WriteLine("EnabledFeature should have been enabled but it isn't because a rule failed?!");
            }

            if (config.IsAvailable(FeaturesStrings.EnabledFeature))
            {
                Console.WriteLine("This feature is clearly enabled - we've just used the string this time - you can define your features anywhere that's good for you");
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
            Console.WriteLine("Press ANY key to exit.");
            Console.ReadKey();
        }
    }

}
