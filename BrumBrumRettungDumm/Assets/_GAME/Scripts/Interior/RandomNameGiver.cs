using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

enum Name
{
    Pepe,
    Juan,
    Maria,
    Jose,
    Pedro,
    Luis,
    Ana,
    Sofia,
    Carlos,
    Laura,
    Carmen,
    Rosa,
    Antonio,
    Miguel,
    Manuel,
    Jesus,
    Javier,
    David,
    Daniel,
    Francisco,
    Alejandro,
    Ramon,
    Oscar,
    Sergio,
    Victor,
    Ruben,
    Jorge,
    Alberto,
    Jenny,
    Flo,
    Manuela,
    Patricia,
    Paula,
    Andrea,
    Natalia,
    Kev, 
    Kevin,
    KevinsonCruso
}

public class RandomNameGiver : MonoBehaviour
{
    [SerializeField] private float offset = 1f;
    private TextMeshProUGUI nameText;
    private GameObject canvas;
    private Transform transformToMoveWith;

    void Awake()
    {
        Name randomName = (Name)Random.Range(0, Enum.GetValues(typeof(Name)).Length);
        gameObject.name = randomName.ToString();
        
        canvas = GetComponentInChildren<Canvas>().gameObject;
        nameText = canvas.GetComponentInChildren<TextMeshProUGUI>();
        nameText.text = gameObject.name;
        transformToMoveWith = GetComponentInChildren<Transform>().GetComponentInChildren<Rigidbody>().transform;
    }

    void Update()
    {
        canvas.transform.position = transformToMoveWith.position + Vector3.up * offset;

        nameText.transform.rotation = Camera.main.transform.rotation;
    }
}