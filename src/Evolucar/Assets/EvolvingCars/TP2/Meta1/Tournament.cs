using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Runner.UnityApp.Car;

public class Tournament : SelectionBase
{
    #region Const

    const int DEFAULT_TOURNAMENT_SIZE = 2;

    #endregion Const


    #region Protected Properties

    protected int Size { get; set; }

    #endregion Protected Properties


    #region Constructors

    /// <summary>
    /// Creates a new Tournament with the default size.
    /// </summary>
    public Tournament() : this(DEFAULT_TOURNAMENT_SIZE)
    {
    }

    /// <summary>
    ///  Creates a new Tournament with a given size.
    /// </summary>
    /// <param name="size">The number of individuals present in the tournament</param>
    public Tournament(int size) : base(DEFAULT_TOURNAMENT_SIZE)
    {
        Size = size;
    }

    #endregion Constructors


    #region Protected Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"></param>
    /// <param name="generation"></param>
    /// <returns></returns>
    protected override IList<IChromosome> PerformSelectChromosomes(int number, Generation generation)
    {

        int i;
        int k;
        var population = generation.Chromosomes.Cast<CarChromosome>().ToList(); // Current Population: We will select individuals from here 
        var parents = new List<IChromosome>(); //List that will return the individuals that will mate, i.e. that will undergo variation

        for (i = 0; i < number; i++)
        {
            var randomIndexes = RandomizationProvider.Current.GetUniqueInts(Size, 0, population.Count);
            var winnerFitness = -1.0f;
            IChromosome winner = null;

            for (k = 0; k < Size; k++)
            {
                var individualIndex = randomIndexes[k];

                if (winner == null || population[individualIndex].Fitness > winnerFitness)
                {
                    winner = population[individualIndex];
                    winnerFitness = population[individualIndex].Fitness;
                }
            }

            parents.Add(winner);
        }

        return parents;
    }

    #endregion Protected Methods
}
