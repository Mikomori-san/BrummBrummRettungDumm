using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;

public class MainMenu : MonoBehaviour
{
    public GameObject joinPlayerMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        joinPlayerMenu.SetActive(true);
        this.joinPlayerMenu.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void OnEnable()
    {
        joinPlayerMenu.SetActive(false);
    }
    private void OnDisable()
    {
        joinPlayerMenu.SetActive(true);
    }
}
