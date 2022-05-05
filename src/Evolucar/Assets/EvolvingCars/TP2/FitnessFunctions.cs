using System;
using GeneticSharp.Runner.UnityApp.Car;

namespace Assets.EvolvingCars.TP2
{
    public static class FitnessFunctions
    {
        public static Func<CarChromosome, float> FITNESS_FUNCTION_DISTANCE = new Func<CarChromosome, float>(c => c.MaxDistance / c.config.RoadLength);
        public static Func<CarChromosome, float> FITNESS_FUNCTION_WHEELS_INVERSE = new Func<CarChromosome, float>(c => 1 / c.NumberOfWheels);
        public static Func<CarChromosome, float> FITNESS_FUNCTION_DISTAN = new Func<CarChromosome, float>(c => c.MaxDistance);
    }
}
