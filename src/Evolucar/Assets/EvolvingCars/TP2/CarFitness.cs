using System.Collections.Concurrent;
using System.Threading;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace GeneticSharp.Runner.UnityApp.Car
{
    public class CarFitness : IFitness
    {
        #region Const

        const int TIME_FITNESS_EVUALTION = 1000;

        #endregion Const


        #region Public Properties

        public BlockingCollection<CarChromosome> ChromosomesToBeginEvaluation { get; private set; }
        public BlockingCollection<CarChromosome> ChromosomesToEndEvaluation { get; private set; }

        #endregion Public Properties


        #region Constructors

        /// <summary>
        /// Creates a new CarFitness.
        /// </summary>
        public CarFitness()
        {
            ChromosomesToBeginEvaluation = new BlockingCollection<CarChromosome>();
            ChromosomesToEndEvaluation = new BlockingCollection<CarChromosome>();
        }

        #endregion Constructors


        #region Public Methods

        /// <summary>
        /// Performs the evaluation of the fitness of a given chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome to be evaluated.</param>
        /// <returns>A number that specifies the chromosome/individual fitness.</returns>
        public double Evaluate(IChromosome chromosome)
        {
            float fitness;
            var c = chromosome as CarChromosome;

            ChromosomesToBeginEvaluation.Add(c);

            do
            {
                Thread.Sleep(TIME_FITNESS_EVUALTION);

                /* 
                 * 
                 * Access to the following information regarding how the car performed in the scenario:
                 * MaxDistanceCurrent: Maximum distance reached by the car;
                 * MaxDistanceTimeCurrent: Time taken to reach the MaxDistanceCurrent;
                 * MaxVelocityCurrent: Maximum Velocity reached by the car;
                 * NumberOfWheels: Number of wheels that the cars has;
                 * CarMass: Weight of the car;
                 * IsRoadComplete: This variable has the value 1 if the car reaches the end of the road, 0 otherwise.
                 * 
                */

                fitness = GeneticAlgorithmConfigurations.fitnessFunction(c);
                c.Fitness = fitness;

            } while (!c.Evaluated);

            ChromosomesToEndEvaluation.Add(c);

            do
            {
                Thread.Sleep(TIME_FITNESS_EVUALTION);
            } while (!c.Evaluated);


            return fitness;
        }
    }

    #endregion Public Methods
}
