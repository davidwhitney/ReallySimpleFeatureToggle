namespace ReallySimpleFeatureToggle.Infrastructure
{
    public interface IRandomNumberGenerator
    {
        int GetRandomNumberBetween(int minValue, int maxValue);
    }
}