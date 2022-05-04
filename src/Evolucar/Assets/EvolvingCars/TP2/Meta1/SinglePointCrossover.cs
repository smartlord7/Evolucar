using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;

namespace GeneticSharp.Runner.UnityApp.Commons
{
    public class SinglePointCrossover : ICrossover
    {
        #region Public Properties
        public int ParentsNumber { get; private set; }
        public int ChildrenNumber { get; private set; }
        public int MinChromosomeLength { get; private set; }
        public bool IsOrdered { get; private set; } // indicating whether the operator is ordered (if can keep the chromosome order).

        #endregion Public Properties


        #region Protected Properties

        protected float crossoverProbability;

        #endregion Protected Properties


        #region Constructors

        public SinglePointCrossover(float crossoverProbability) : this(2, 2, 2, true)
        {
            this.crossoverProbability = crossoverProbability;
        }

        public SinglePointCrossover(int parentsNumber, int offSpringNumber, int minChromosomeLength, bool isOrdered)
        {
            ParentsNumber = parentsNumber;
            ChildrenNumber = offSpringNumber;
            MinChromosomeLength = minChromosomeLength;
            IsOrdered = isOrdered;
        }

        #endregion Constructors


        #region Public Methods

        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            int i;
            var parent1 = parents[0];
            var parent2 = parents[1];
            var offspring1 = parent1.CreateNew();
            var offspring2 = parent2.CreateNew();

            if (RandomizationProvider.Current.GetDouble() <= crossoverProbability)
            {
                var cutPoint = RandomizationProvider.Current.GetInt(1, parent1.Length);

                for (i = 0; i < cutPoint; i++)
                {
                    offspring1.ReplaceGene(i, parent2.GetGene(i));
                    offspring2.ReplaceGene(i, parent1.GetGene(i));
                }
            }

            return new List<IChromosome> { offspring1, offspring2 };

        }

        #endregion Public Methods
    }
}
