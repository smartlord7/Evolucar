using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;

public class SinglePointMutation : IMutation
{
    #region Public Properties

    public bool IsOrdered { get; private set; } // indicating whether the operator is ordered (if can keep the chromosome order).

    #endregion Public Properties


    #region Constructors

    /// <summary>
    /// Creates a new SinglePointMutation.
    /// </summary>
    public SinglePointMutation()
    {
        IsOrdered = true;
    }

    #endregion Constructors


    #region Public Methods

    /// <summary>
    /// Performs the mutation of a chromosome, with a certain probability, by looping through its genes.
    /// </summary>
    /// <param name="chromosome">The chromosome to be mutated.</param>
    /// <param name="probability">A number, between 0 and 1, that specifies if the current gene is to be mutatded.</param>
    public void Mutate(IChromosome chromosome, float probability)
    {
        int i;

        for (i = 0; i < chromosome.Length; i++)
        {
            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                var geneValue = chromosome.GetGene(i).Value;

                if (geneValue.Equals(1))
                {
                    chromosome.ReplaceGene(i, new Gene(0));
                }
                else
                {
                    chromosome.ReplaceGene(i, new Gene(1));
                }
            }
        }

    }

    #endregion Public Methods
}
