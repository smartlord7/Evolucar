﻿using System;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
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

            var fitness = f * c.MaxDistanceCurrent + 7 * c.MaxVelocityCurrent - 0.5f * c.CarMass - 5 * c.MaxDistanceTimeCurrent;

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

            var fitness = f * c.MaxDistanceCurrent + 12 * c.MaxVelocityCurrent - 1.5f * c.CarMass - 7 * c.MaxDistanceTimeCurrent;

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
            if (c.MaxDistanceCurrent > 360 && !c.IsRoadComplete)
            {
                return 0;
            }

            var f = 1f;

            f *= c.MaxDistanceCurrent % 360;

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

        public static float FITNESS_FUNCTION_7(CarChromosome c)
        {
            var f = 0.0f;

            if (c.IsRoadComplete)
            {
                f += 0.4f;
            }
            else
            {
                f += 0.4f * (c.MaxDistanceCurrent / c.config.RoadLength);
            }

            var massFactor = 0.3f * (1.0f - c.CarMass / GetMaxMass(c));
            f += massFactor;

            var velocityFactor = 0.2f * (c.MaxVelocityPrevious == 0 ? 1 : Mathf.Clamp(c.MaxVelocityCurrent / c.MaxVelocityPrevious, 0.0f, 1.0f));
            f += velocityFactor;

            var nWheelsFactor =  0.1f * MathUtil.Gaussian(c.NumberOfWheels, 3.0f, 2.0f);
            f += nWheelsFactor;

            return f * 100;
        }

        private static float GetMaxMass(CarChromosome c) {
                var phenotypes = c.GetPhenotypes();
                var points = phenotypes.Select(p => p.Vector).ToArray();
                var nWheels = c.GetPhenotypes().Length;
                var vectorMagBits = CarVectorPhenotypeEntity.VectorSizeBits;
                var wheelRadiusBits = CarVectorPhenotypeEntity.WheelRadiusBits;
                var maxMass = 0.0f;

                maxMass += (float) Math.Pow(2, wheelRadiusBits) - 1;
                maxMass += 1.0f + (float) + points.Sum(p => Math.Pow(2, vectorMagBits)) * 2.0f;

            return maxMass;
        }
    }
}