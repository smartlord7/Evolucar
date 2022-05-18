using GeneticSharp.Domain;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Infrastructure.Framework.Threading;
using UnityEngine;

namespace GeneticSharp.Runner.UnityApp.Car
{
    public class CarSampleController : SampleControllerBase
    {
        private static CarSampleConfig s_config;

        private int NumberOfSimultaneousEvaluations = 50;
        public Vector2Int SimulationsGrid = new Vector2Int(2, 2);
        public Vector3 EvaluationDistance = new Vector3(0, 0, 2);

        public Object EvaluationPrefab;
        public CarSampleConfig Config;

        private CarFitness m_fitness;
        private Vector3 m_lastPosition;
        private PrefabPool m_evaluationPool;

        public static void SetConfig(CarSampleConfig config)
        {
            s_config = config;
        }

        private void Awake()
        {
            if (s_config != null)
            {
                Config = s_config;

            }
        }

        protected override GeneticAlgorithm CreateGA()
        {
            NumberOfSimultaneousEvaluations = SimulationsGrid.x * SimulationsGrid.y;
            m_fitness = new CarFitness();
            CarChromosome chromosome = new CarChromosome(Config);


            var crossoverOperator = GeneticAlgorithmConfigurations.crossoverOperator;
            var mutationOperator = GeneticAlgorithmConfigurations.mutationOperator;
            var parentSelection = GeneticAlgorithmConfigurations.parentSelection;
            var terminationCondition = GeneticAlgorithmConfigurations.terminationCondition;
            var survivorSelection = GeneticAlgorithmConfigurations.survivorSelection;




            Population population = new Population(NumberOfSimultaneousEvaluations, NumberOfSimultaneousEvaluations, chromosome)
            {
                GenerationStrategy = new PerformanceGenerationStrategy()
            };

            GeneticAlgorithm ga = new GeneticAlgorithm(population, m_fitness, parentSelection, crossoverOperator, mutationOperator)
            {
                Reinsertion = survivorSelection
            };

            ga.Termination = terminationCondition;
            ga.CrossoverProbability = 1.0f; // DO NOT CHANGE!!!
            ga.MutationProbability = GeneticAlgorithmConfigurations.mutationProbability;

            ga.TaskExecutor = new ParallelTaskExecutor
            {
                MinThreads = population.MinSize,
                MaxThreads = population.MaxSize * 2
            };
            ga.GenerationRan += delegate
            {
                m_lastPosition = Vector3.zero;
                m_evaluationPool.ReleaseAll();
            };

            return ga;
        }

        protected override void StartSample()
        {
            ChromosomesCleanupEnabled = false;
            m_lastPosition = Vector3.zero;
            m_evaluationPool = new PrefabPool(EvaluationPrefab);
        }

        protected override void UpdateSample()
        {
            if (GA.IsRunning)
            {
                // end evaluation.
                while (m_fitness.ChromosomesToEndEvaluation.Count > 0)
                {
                    m_fitness.ChromosomesToEndEvaluation.TryTake(out CarChromosome c);
                    c.Evaluated = true;
                }


                // in evaluation.
                while (m_fitness.ChromosomesToBeginEvaluation.Count > 0)
                {
                    m_fitness.ChromosomesToBeginEvaluation.TryTake(out CarChromosome c);
                    c.Evaluated = false;
                    c.MaxDistanceCurrent = 0;

                    var evaluation = m_evaluationPool.Get(m_lastPosition);
                    evaluation.name = c.ID;

                    var road = evaluation.GetComponentInChildren<RoadController>();
                    road.Build(Config);

                    var car = evaluation.GetComponentInChildren<CarController>();
                    car.transform.position = m_lastPosition;
                    car.SetChromosome(c, Config);

                    m_lastPosition += EvaluationDistance;
                }
            }
        }
    }
}