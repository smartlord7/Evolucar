using System;
using System.IO;
using System.Linq;
using System.Threading;
using GeneticSharp.Domain;
using GeneticSharp.Runner.UnityApp.Car;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class SampleControllerBase : MonoBehaviour
{
    #region Const

    const string DELIMITER_OUTPUT = ";";
    const string NAME_FILE_EVOLUTION = "EvolutionLog.csv";

    #endregion Const

    #region Private Properties


    private Thread m_gaThread;
    private Text m_generationText;
    private Text m_fitnessText;
    private Text m_previousGenerationText;
    private Text m_previousFitnessText;
    private double m_previousBestFitness;
    private double m_previousAverageFitness;
    private string folderName;

    #endregion Private Properties

    #region Public Properties

    protected GeneticAlgorithm GA { get; private set; }
    protected bool ChromosomesCleanupEnabled { get; set; }
    protected bool ShowPreviousInfoEnabled { get; set; } = true;

    #endregion Public Properties


    #region Public Properties

    public Rect Area { get; private set; }

    #endregion Public Properties


    #region Private Methods

    private void Start()
    {
        Application.runInBackground = true;
        var sampleArea = GameObject.Find("SampleArea");
        Area = sampleArea == null
            ? Camera.main.rect
            : sampleArea.GetComponent<RectTransform>().rect;

        var generationTextGO = GameObject.Find("CurrentInfo/Background/GenerationText");

        if (generationTextGO != null)
        {
            var fitnessTextGO = GameObject.Find("CurrentInfo/Background/FitnessText");
            m_generationText = generationTextGO.GetComponent<Text>();
            m_fitnessText = fitnessTextGO.GetComponent<Text>();

            m_previousGenerationText = GameObject.Find("PreviousInfo/Background/GenerationText").GetComponent<Text>();
            m_previousFitnessText = GameObject.Find("PreviousInfo/Background/FitnessText").GetComponent<Text>();
            m_previousGenerationText.text = string.Empty;
            m_previousFitnessText.text = string.Empty;
        }

        CreateStatsFolder();

        if (m_generationText != null)
        {
            m_generationText.text = string.Empty;
            m_fitnessText.text = string.Empty;
        }

        GA = CreateGA();
        GA.GenerationRan += delegate
        {
            UpdateStatistics();

            if (ChromosomesCleanupEnabled)
            {
                foreach (var c in GA.Population.CurrentGeneration.Chromosomes)
                {
                    c.Fitness = null;
                }
            }
        };

        StartSample();

        m_gaThread = new Thread(() =>
        {
            try
            {
                Thread.Sleep(1000);
                GA.Start();
            }
            catch (Exception ex)
            {
                if (ex.Message.Length > 0)
                {
                    Debug.LogError($"GA thread error: {ex.Message}");
                }

            }
        });

        m_gaThread.Start();
    }

    private void Update()
    {
        if (m_generationText != null && GA?.Population.CurrentGeneration != null)
        {
            var currentGeneration = GA.Population.CurrentGeneration;
            var chromosomes = GA.Population.CurrentGeneration.Chromosomes;
            var averageFitness = chromosomes.Average(c => c.Fitness ?? 0);
            var bestFitness = chromosomes.Max(c => c.Fitness ?? 0);

            UpdateTexts(
                m_generationText,
                m_fitnessText,
                GA.GenerationsNumber,
                bestFitness,
                averageFitness);

            if (ShowPreviousInfoEnabled && GA.GenerationsNumber > 1)
            {
                UpdateTexts(
                    m_previousGenerationText,
                    m_previousFitnessText,
                    GA.GenerationsNumber - 1,
                    m_previousBestFitness,
                    m_previousAverageFitness);
            }

        }

        UpdateSample();
    }

    private void OnDestroy()
    {
        GA.Stop();
        m_gaThread.Abort();

        if (m_generationText != null)
        {
            m_generationText.text = String.Empty;
            m_fitnessText.text = String.Empty;

            m_previousGenerationText.text = String.Empty;
            m_previousFitnessText.text = String.Empty;
        }
    }

    private void UpdateTexts(Text generationText, Text fitnessText, int generationsNumber, double bestFitness, double averageFitness)
    {
        generationText.text = $"Generation: {generationsNumber}";
        fitnessText.text = $"Best Fitness: {bestFitness:N2}\nPopAverage: {averageFitness:N2}";
    }

    private void UpdateStatistics()
    {
        m_previousBestFitness = GA.BestChromosome.Fitness.Value;
        m_previousAverageFitness = GA.Population.CurrentGeneration.Chromosomes.Average(c => c.Fitness.Value);

        var best = GA.BestChromosome as CarChromosome;
        string pathToFile = $"{this.folderName}/{NAME_FILE_EVOLUTION}";
        string generationInfo = $"{GA.GenerationsNumber}{DELIMITER_OUTPUT}{m_previousBestFitness}{DELIMITER_OUTPUT}{m_previousAverageFitness}{DELIMITER_OUTPUT}{best.MaxDistanceCurrent}{DELIMITER_OUTPUT}{best.MaxDistanceTimeCurrent}{DELIMITER_OUTPUT}{best.NumberOfWheels}{DELIMITER_OUTPUT}{best.CarMass}{DELIMITER_OUTPUT}{best.IsRoadComplete}" + Environment.NewLine;


        if (!File.Exists(pathToFile))
        {
            string header = $"Generation{DELIMITER_OUTPUT}BestFitness{DELIMITER_OUTPUT}AverageFitnessPopulation{DELIMITER_OUTPUT}BestMaxDistance{DELIMITER_OUTPUT}BestMaxDistanceTime{DELIMITER_OUTPUT}BestNumberOfWheels{DELIMITER_OUTPUT}BestCarMass{DELIMITER_OUTPUT}BestIsRoadComplete" + Environment.NewLine;
            Debug.Log(header);

            File.WriteAllText(pathToFile, header);
        }

        Debug.Log(generationInfo);
        File.AppendAllText(pathToFile, generationInfo);
        DumpOverallBest();
    }

    private void DumpOverallBest()
    {
        string pathToFile = this.folderName + "/OverallBestGenotype.txt";
        File.WriteAllText(pathToFile, GA.BestChromosome.ToString());
    }

    private void CreateStatsFolder()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string configName = "-crossProb-" + GeneticAlgorithmConfigurations.crossoverProbability.ToString() + "-elite-" + GeneticAlgorithmConfigurations.eliteSize.ToString() + "-mutProb-" + GeneticAlgorithmConfigurations.mutationProbability.ToString() + "-tournamenteSize-" + GeneticAlgorithmConfigurations.tournamentSize.ToString() + "-fitnessFunction-" + GeneticAlgorithmConfigurations.fitnessFunction.Method.Name;
        this.folderName = "Results/" + sceneName + "-" + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + configName;
        Directory.CreateDirectory(this.folderName);
    }

    #endregion Private Methods


    #region Protected Methods

    protected virtual void StartSample()
    {
    }

    protected abstract GeneticAlgorithm CreateGA();

    protected virtual void UpdateSample()
    {
    }

    protected void SetFitnessText(string text)
    {
        if (m_fitnessText != null)
        {
            m_fitnessText.text = text;
        }
    }

    #endregion Protected Methods
}
