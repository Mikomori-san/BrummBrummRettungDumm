using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

enum MaleNames
{
    Pepe,
    Juan,
    Jose,
    Pedro,
    Luis,
    Carlos,
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
    Flo,
    Kev, 
    Kevin,
    Wilson,
    Fabian,
    Nikolai,
    Ostav,
    Ulric,
    Vicent,
    York,
    Zephyrus,
    John,
    Michael,
    William,
    James,
    Joseph,
    Benjamin,
    Samuel,
    Matthew,
    Christopher,
    Andrew,
    Anthony,
    Joshua,
    Nicholas,
    Ryan,
    Jacob,
    Tyler,
    Alexander,
    Ethan,
    Noah,
    Mason,
    Liam,
    Aiden,
    Jayden,
    Elijah,
    Gabriel,
    Logan,
    Caleb,
    Jackson,
    Dylan,
    Lucas,
    Isaac,
    Owen,
    Carter,
    Connor,
    Luke,
    Brayden,
    Jack,
    Brandon,
    Nathan,
    Hunter,
    Cameron,
    Thomas,
    Aaron,
    Adrian,
    Austin,
    Evan,
    Jordan,
    Justin,
    Kyle,
    Gavin,
    Zachary,
    Dominic,
    Adam,
    Ian,
    Tristan,
    Cole,
    Levi,
    Cooper,
    Xavier,
    Ayden,
    Chase,
    Blake,
    Carson,
    Henry,
    Nolan,
    Peter,
    Riley,
    Sebastian,
    Theodore,
    Vincent
}

enum FemaleNames
{
    Maria,
    Ana,
    Sofia,
    Laura,
    Carmen,
    Rosa,
    Ramona,
    Jenny,
    Manuela,
    Patricia,
    Paula,
    Andrea,
    Natalia,
    Alice,
    Olivia,
    Emma,
    Ava,
    Sophia,
    Isabella,
    Mia,
    Amelia,
    Harper,
    Evelyn,
    Abigail,
    Emily,
    Charlotte,
    Elizabeth,
    Avery,
    Ella,
    Madison,
    Scarlett,
    Grace,
    Lily,
    Chloe,
    Victoria,
    Riley,
    Aria,
    Zoe,
    Penelope,
    Nora,
    Layla,
    Mila,
    Aurora,
    Hannah,
    Savannah,
    Addison,
    Brooklyn,
    Lillian,
    Natalie,
    Zoey,
    Hazel,
    Audrey,
    Claire,
    Ariana,
    Skylar,
    Paisley,
    Naomi,
    Elena,
    Sarah,
    Gabriella,
    Hailey,
    Taylor,
    Aaliyah,
    Alexa,
    Kennedy,
    Maya,
    Peyton,
    Serenity,
    Arianna,
    Violet,
    Caroline,
    Kaylee,
    Jasmine,
    Julia,
    Valentina,
    Reagan,
    Eleanor,
    Mackenzie,
    Quinn,
    Isabelle,
    Faith,
    Ashley,
    Destiny,
    Vanessa,
    Angelina,
    Hope,
    Trinity,
    Clara,
    Diana,
    Stella,
    Ruby,
    Alexis,
    Lydia,
    Jocelyn,
    Jade,
    Gabrielle,
    Autumn,
    Melody,
    Cecilia,
    Olive,
    Fernanda
}


public class RandomNameGiver : MonoBehaviour
{
    [SerializeField] private float offset = 1f;
    [SerializeField] private bool isMale;
    private TextMeshProUGUI nameText;
    private GameObject canvas;
    private Transform transformToMoveWith;

    void Awake()
    {
        string randomName;
        if (isMale)
        {
            MaleNames name = (MaleNames)Random.Range(0, Enum.GetValues(typeof(MaleNames)).Length);
            randomName = name.ToString();
        }
        else
        {
            FemaleNames name = (FemaleNames)Random.Range(0, Enum.GetValues(typeof(FemaleNames)).Length);
            randomName = name.ToString();
        }

        gameObject.name = randomName;
        
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