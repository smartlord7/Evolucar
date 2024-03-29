using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;

public class Elitism : ReinsertionBase
{
    #region Protected Properties

    protected int eliteSize = 0;

    #endregion Protected Properties


    #region Constructors

    public Elitism(int eliteSize) : base(false, false)
    {
        this.eliteSize = eliteSize;
    }

    #endregion Constructors


    #region Protected Methods

    /// <summary>
    /// Selects the chromosomes of the best <eliteSize> individuals of the current generation, based on their fitness.
    /// </summary>
    /// <param name="population">The global population of the experiment</param>
    /// <param name="offspring">The generated offspring of the current generation</param>
    /// <param name="parents">The parents of the current generation</param>
    /// <returns>The selected chromosomes.</returns>
    protected override IList<IChromosome> PerformSelectChromosomes(IPopulation population, IList<IChromosome> offspring, IList<IChromosome> parents)
    {
        var old_population = population.CurrentGeneration.Chromosomes.OrderByDescending(p => p.Fitness).ToList(); //previous population sorted by fitness
        int i;

        for (i = 0; i < eliteSize; i++)
        {
            offspring[i] = old_population[i];
        }

        return offspring;
    }

    #endregion Protected Methods
}
