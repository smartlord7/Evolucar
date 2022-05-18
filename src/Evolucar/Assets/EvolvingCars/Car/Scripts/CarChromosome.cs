using System;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Runner.UnityApp.Commons;

namespace GeneticSharp.Runner.UnityApp.Car
{
    [Serializable]
    public class CarChromosome : BitStringChromosome<CarVectorPhenotypeEntity>
    {
        public readonly CarSampleConfig config;
        public CarChromosome(CarSampleConfig config)
        {
            this.config = config;

            var phenotypeEntities = new CarVectorPhenotypeEntity[config.VectorsCount];

            for (int i = 0; i < phenotypeEntities.Length; i++)
            {
                phenotypeEntities[i] = new CarVectorPhenotypeEntity(config, i);
            }

            SetPhenotypes(phenotypeEntities);
            CreateGenes();
        }

        public string ID { get; } = System.Guid.NewGuid().ToString();

        public bool Evaluated { get; set; }
        public float MaxDistanceCurrent { get; set; }
        public float MaxDistancePrevious { get; set; }
        public float MaxDistanceTimeCurrent { get; set; }
        public float MaxDistanceTimePrevious { get; set; }
        new public float Fitness { get; set; }
        public float NumberOfWheels { get; set; }
        public float CarMass { get; set; }
        public bool IsRoadComplete { get; set; } = false;
        public float MaxVelocityCurrent => MaxDistanceTimeCurrent > 0 ? MaxDistanceCurrent / MaxDistanceTimeCurrent : 0;
        public float MaxVelocityPrevious => MaxDistanceTimePrevious > 0 ? MaxDistancePrevious / MaxDistanceTimePrevious : 0;

        public override IChromosome CreateNew()
        {
            return new CarChromosome(config);
        }
    }
}
