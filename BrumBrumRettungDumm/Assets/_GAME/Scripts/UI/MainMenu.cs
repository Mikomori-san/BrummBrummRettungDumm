using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;

public class MainMenu : MonoBehaviour
{
    public GameObject joinMenu;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToJoinMenu()
    {
        joinMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void OnEnable()
    {
        joinMenu.SetActive(false);
    }
}
