using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance { get { return instance; } }
    
    private const float MAX_TIME = 10;
    private const short ADDED_TIME = 30;
    private float time = MAX_TIME;
    private bool isOver = false;
    
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    public string EndMenuScene;
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

    private void Start()
    {

    }

    void Update()
    {
        if (time <= 0)
        {
            time = 0;
            if(!isOver)
            {
                isOver = true;
                ScoreSystem.Instance.AddScoreRemainingPatient(PatientManager.Instance.allPatients.Count);
                SceneManager.LoadScene(EndMenuScene);
                // End the game
            }
        }
        else
        {
            time -= Time.deltaTime;
        }
        
        if((short)(time % 60) < 10)
            timeText.text = (short)(time / 60) + ":0" + (short)(time % 60);
        else
            timeText.text = (short)(time / 60) + ":" + (short)(time % 60);
    }
    
    public void AddTime()
    {
        time -= ADDED_TIME;
    }
}