using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    private float multiplier = 1;
    private float score = 0;
    private const short PILL_SCORE = 100;
    private const short DEFIBRILATOR_SCORE = 150;
    private const short DELIVERY_SCORE = 500;
    private const short REMAINING_PATIENT_SCORE = 50;
    private const short PATIENT_DEATH_SCORE = -1000;
    
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI multiplierText;

    public static string scoreSaveFile = "score.csv";

    private static ScoreSystem instance;
    
    public static ScoreSystem Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        scoreText.text = score.ToString("F0");
        multiplierText.text = "x" + multiplier.ToString("F2");
    }

    private void OnDisable()
    {
        WriteScoreToFile(score);
    }

    public float GetScore()
    {
        return score;
    }
    
    public void AddScorePill()
    {
        score += PILL_SCORE * multiplier;
    }
    
    public void AddScoreDefibrilator()
    {
        score += DEFIBRILATOR_SCORE * multiplier;
    }
    
    public void AddScoreDelivery(float patientHealthPercent)
    {
        multiplier += patientHealthPercent;
        score += DELIVERY_SCORE * multiplier;
    }
    
    public void AddScoreRemainingPatient(int remainingPatientCount)
    {
        score += REMAINING_PATIENT_SCORE * remainingPatientCount * multiplier;
    }
    
    public void AddScorePatientDeath()
    {
        score += PATIENT_DEATH_SCORE;
        multiplier = 1;
    }
    public static void WriteScoreToFile(float score)
    {
        string path = Application.dataPath + "/" + scoreSaveFile;
        //If file doesn't exist, create it
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
            File.WriteAllText(path, "Score; Highscore\n0; 0\n");
        }

        //Write score and highscore to file
        try
        {
            string[] lines = File.ReadAllLines(path);
            string[] values = lines[1].Split(';');
            int highscore = int.Parse(values[1]);
            if (score > highscore)
            {
                highscore = (int)score;
            }
            string newLine = score + "; " + highscore;
            lines[1] = newLine;
            File.WriteAllLines(path, lines);
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading or writing to score-file: " + e.Message);
        }
    }
    public static void GetScoresFromFile(out int score, out int highscore)
    {
        string path = Application.dataPath + "/" + scoreSaveFile;
        score = 0;
        highscore = 0;
        if(File.Exists(path))
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                string[] values = lines[1].Split(';');
                score = int.Parse(values[0]);
                highscore = int.Parse(values[1]);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading from score-file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Score-file not found");
        }
    }
}
