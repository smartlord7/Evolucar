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
            var fitness = c.MaxDistance;

            if (c.NumberOfWheels < 2)
            {
                c.NumberOfWheels -= c.NumberOfWheels * 25;
            } else if (c.NumberOfWheels > 4)
            {
                c.NumberOfWheels -= (c.NumberOfWheels - 4) * 10;
            }

            return fitness;
        }

        public static float FITNESS_FUNCTION_4(CarChromosome c)
        {
            var f = 1f;

            if (c.IsRoadComplete)
            {
                f = 4;
            }

            var fitness = f * c.MaxDistance + 7 * c.MaxVelocity - 0.5f * c.CarMass - 5 * c.MaxDistanceTime;

            if (c.NumberOfWheels <= 1)
            {
                c.NumberOfWheels -= 300;
            } else if (c.NumberOfWheels == 2)
            {
                c.NumberOfWheels -= 50;
            }


            return fitness;
        }

        public static float FITNESS_FUNCTION_5(CarChromosome c)
        {
            var f = 1f;

            if (c.IsRoadComplete)
            {
                f = 4;
            }

            var fitness = f * c.MaxDistance + 12 * c.MaxVelocity - 1.5f * c.CarMass - 7 * c.MaxDistanceTime;

            if (c.NumberOfWheels <= 1)
            {
                c.NumberOfWheels -= 400;
            }
            else if (c.NumberOfWheels == 2)
            {
                c.NumberOfWheels -= 25;
            }

            return fitness;
        }

        public static float FITNESS_FUNCTION_6(CarChromosome c)
        {
            if (c.MaxDistance > 360 && !c.IsRoadComplete)
            {
                return 0;
            }

            var f = 1f;

            f *= c.MaxDistance % 360;

            if (f >= 230)
            {
                f += (f - 230) * 30;
            }

            var f2 = 0f;

            if (c.IsRoadComplete)
            {
                f2 = 1;
            }

            return f + f2 * 1000;
        }
    }
}
