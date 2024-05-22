using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.InputSystem.Scripts.Interior
{
    public class DefibrilatorUI : MonoBehaviour
    {
        [SerializeField] private GameObject defibrilatorUi;
        [SerializeField] private DefibrilatorTask defibrilatorTask;
        bool isActive = false;
        private Slider defiSlider;

        private void Start()
        {
            defiSlider = defibrilatorUi.GetComponentInChildren<Slider>();
            GetComponent<Canvas>().worldCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (isActive)
            {
                defiSlider.value = defibrilatorTask.GetProgress();
            }
        }
        
        public void ShowDefibrilatorUI()
        {
            isActive = true;
            defibrilatorUi.SetActive(isActive);
        }
        
        public void HideDefibrilatorUI()
        {
            isActive = false;
            defibrilatorUi.SetActive(isActive);
        }
    }
}