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

    protected override IList<IChromosome> PerformSelectChromosomes(IPopulation population, IList<IChromosome> offspring, IList<IChromosome> parents)
        => population.CurrentGeneration.Chromosomes.OrderByDescending(p => p.Fitness).Take(eliteSize) as IList<IChromosome>;

    #endregion Protected Methods
}
