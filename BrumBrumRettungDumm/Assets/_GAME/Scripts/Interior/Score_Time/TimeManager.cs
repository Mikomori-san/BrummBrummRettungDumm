using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance { get { return instance; } }
    
    private const float MAX_TIME = 300;
    private const short ADDED_TIME = 30;
    private float time = MAX_TIME;
    private bool isOver = false;
    
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
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
        DontDestroyOnLoad(gameObject);
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