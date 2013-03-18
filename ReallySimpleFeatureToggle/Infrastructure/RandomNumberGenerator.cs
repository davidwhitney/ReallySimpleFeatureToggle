using System;

namespace ReallySimpleFeatureToggle.Infrastructure
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _generator;

        public RandomNumberGenerator()
        {
            _generator = new Random();
        }

        public int GetRandomNumberBetween(int minValue, int maxValue)
        {
            return _generator.Next(minValue, maxValue);
        }
    }
}