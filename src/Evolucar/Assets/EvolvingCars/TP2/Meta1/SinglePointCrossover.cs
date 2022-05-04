using System.Collections.Generic;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;

namespace GeneticSharp.Runner.UnityApp.Commons
{
    public class SinglePointCrossover : ICrossover
    {
        #region Const

        const int DEFAULT_PARENTS_NUMBER = 2;
        const int DEFAULT_OFFSPRING_NUMBER = 2;
        const int DEFAULT_MIN_CHROMOSOME_LENGTH = 2;

        #endregion Const

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

        /// <summary>
        /// Creates a new SinglePointCrossover with the specified crossover probability and the remaining parameters as default.
        /// </summary>
        /// <param name="crossoverProbability">A number, between 0 and 1, that specifies the probability of the crossover ocurrence</param>
        public SinglePointCrossover(float crossoverProbability) : this(DEFAULT_PARENTS_NUMBER, DEFAULT_OFFSPRING_NUMBER, DEFAULT_MIN_CHROMOSOME_LENGTH, true)
        {
            this.crossoverProbability = crossoverProbability;
        }

        /// <summary>
        /// Creates a new custom SinglePointCrossover.
        /// </summary>
        /// <param name="parentsNumber">The number of individual from which the offspring genotype will be based in.</param>
        /// <param name="offSpringNumber">The number of generated individuals by the parents.</param>
        /// <param name="minChromosomeLength">The minimum number of genes in each offspring chromosome.</param>
        /// <param name="isOrdered">Specifies if the operator is ordered (if it can keep the chromosome order).</param>
        public SinglePointCrossover(int parentsNumber, int offSpringNumber, int minChromosomeLength, bool isOrdered)
        {
            ParentsNumber = parentsNumber;
            ChildrenNumber = offSpringNumber;
            MinChromosomeLength = minChromosomeLength;
            IsOrdered = isOrdered;
        }

        #endregion Constructors


        #region Public Methods

        /// <summary>
        /// Performs the crossing over a list of chromosomes.
        /// </summary>
        /// <param name="parents">The chromosomes of the individuals from which the offspring genotype will be based in</param>
        /// <returns>The generated chromosomes</returns>
        public IList<IChromosome> Cross(IList<IChromosome> parentsChromosomes)
        {
            int i;
            var parent1 = parentsChromosomes[0];
            var parent2 = parentsChromosomes[1];
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
