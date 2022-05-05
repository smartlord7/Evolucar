using System;
using GeneticSharp.Runner.UnityApp.Car;

namespace Assets.EvolvingCars.TP2
{
    public static class FitnessFunctions
    {
        public static float FITNESS_FUNCTION_1(CarChromosome c)
            => c.MaxDistance / c.config.RoadLength;

        public static float FITNESS_FUNCTION_2(CarChromosome c)
            => 1 / c.NumberOfWheels;

        public static float FITNESS_FUNCTION_3(CarChromosome c)
        {
            var fitness = c.MaxDistance / c.config.RoadLength;

            if (c.NumberOfWheels < 2)
            {
                c.NumberOfWheels -= c.NumberOfWheels * 0.25f;
            } else if (c.NumberOfWheels > 4)
            {
                c.NumberOfWheels -= (c.NumberOfWheels - 4) * 0.1f;
            }

            return fitness;
        }
    }
}
