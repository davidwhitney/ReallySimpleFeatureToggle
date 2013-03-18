namespace ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours
{
    public class FeatureShouldBeEnabled : ReturnBoolWhenFeatureNotConfigured 
    {
        public FeatureShouldBeEnabled() : base(true) { }
    }
}