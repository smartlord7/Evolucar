using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;

public class SinglePointMutation : IMutation
{
    #region Public Properties

    public bool IsOrdered { get; private set; } // indicating whether the operator is ordered (if can keep the chromosome order).

    #endregion Public Properties


    #region Constructors

    public SinglePointMutation()
    {
        IsOrdered = true;
    }

    #endregion Constructors


    #region Public Methods

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
