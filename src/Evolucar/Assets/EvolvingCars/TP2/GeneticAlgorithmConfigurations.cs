using System;
using Assets.EvolvingCars.TP2;
using GeneticSharp.Runner.UnityApp.Car;
using GeneticSharp.Runner.UnityApp.Commons;

public static class GeneticAlgorithmConfigurations
{
    #region Public Static Properties
    public static float crossoverProbability = 0.9f;
    public static float mutationProbability = 0.05f;
    public static int tournamentSize = 5;
    public static int maximumNumberOfGenerations = 30;
    public static int eliteSize = 2;
    public static Func<CarChromosome, float> fitnessFunction = FitnessFunctions.FITNESS_FUNCTION_3;
    public static SinglePointCrossover crossoverOperator = new SinglePointCrossover(crossoverProbability);
    public static SinglePointMutation mutationOperator = new SinglePointMutation();
    public static Tournament parentSelection = new Tournament(tournamentSize);
    public static Elitism survivorSelection = new Elitism(eliteSize);
    public static GenerationsTermination terminationCondition = new GenerationsTermination(maximumNumberOfGenerations);

    #endregion Public Static Properties
}
