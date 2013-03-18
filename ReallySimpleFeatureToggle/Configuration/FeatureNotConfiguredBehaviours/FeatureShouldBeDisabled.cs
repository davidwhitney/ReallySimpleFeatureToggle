namespace ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours
{
    public class FeatureShouldBeDisabled : ReturnBoolWhenFeatureNotConfigured 
    {
        public FeatureShouldBeDisabled() : base(false) { }
    }
}