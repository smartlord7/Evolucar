using System;
using System.Linq;
using GeneticSharp.Runner.UnityApp.Car;
using UnityEngine;

namespace Assets.EvolvingCars.TP2
{
    public static class FitnessFunctions
    {
        public static float FITNESS_FUNCTION_1(CarChromosome c)
            => c.MaxDistanceCurrent / c.config.RoadLength;

        public static float FITNESS_FUNCTION_2(CarChromosome c)
            => 1 / c.NumberOfWheels;

        public static float FITNESS_FUNCTION_3(CarChromosome c)
        {
            var fitness = c.MaxDistanceCurrent;

            if (c.NumberOfWheels < 2)
            {
                fitness -= c.NumberOfWheels * 25;
            }
            else if (c.NumberOfWheels > 4)
            {
                fitness -= (c.NumberOfWheels - 4) * 10;
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

            var fitness = f * c.MaxDistanceCurrent + 7 * c.MaxVelocityCurrent - 0.5f * c.CarMass - 5 * c.MaxDistanceTimeCurrent;

            if (c.NumberOfWheels <= 1)
            {
                c.NumberOfWheels -= 300;
            }
            else if (c.NumberOfWheels == 2)
            {
                c.NumberOfWheels -= 50;
            }


            return fitness;
        }

        public static float FITNESS_FUNCTION_5(CarChromosome c)
        {
            if (c.MaxDistanceCurrent > 360 && !c.IsRoadComplete)
            {
                return 0;
            }

            var f = c.MaxDistanceCurrent;

            if (f > 230)
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

        public static float FITNESS_FUNCTION_6(CarChromosome c)
        {
            var f = 0.0f;

            if (c.MaxDistanceCurrent > c.config.RoadLength)
            {
                return 0.0f;
            }

            if (c.IsRoadComplete)
            {
                f += 0.5f;
            }
            else
            {
                f += 0.5f * (c.MaxDistanceCurrent / c.config.RoadLength);
            }

            var massFactor = 0.3f * (1.0f - c.CarMass / GetMaxMass(c));
            f += massFactor;

            var velocityFactor = 0.15f * (c.MaxVelocityPrevious == 0 ? 1 : Mathf.Clamp(c.MaxVelocityCurrent / c.MaxVelocityPrevious, 0.0f, 1.0f));
            f += velocityFactor;

            var nWheelsFactor = 0.05f * MathUtil.Gaussian(c.NumberOfWheels, 3.0f, 2.0f);
            f += nWheelsFactor;

            return f * 100;
        }

        private static float GetMaxMass(CarChromosome c)
        {
            var phenotypes = c.GetPhenotypes();
            var points = phenotypes.Select(p => p.Vector).ToArray();
            var nWheels = c.GetPhenotypes().Length;
            var vectorMagBits = CarVectorPhenotypeEntity.VectorSizeBits;
            var wheelRadiusBits = CarVectorPhenotypeEntity.WheelRadiusBits;
            var maxMass = 0.0f;

            maxMass += phenotypes.Length * (float)Math.Pow(2, wheelRadiusBits) - 1;
            maxMass += 1.0f + (float)+points.Sum(p => Math.Pow(2, vectorMagBits)) * 2.0f;

            return maxMass;
        }
    }
}
