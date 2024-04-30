using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverLocation : MonoBehaviour
{
    [SerializeField] private string ambulanceTag;
    [SerializeField] private GameObject interiorDeliveryZone;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(ambulanceTag))
        {
            interiorDeliveryZone.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ambulanceTag))
        {
            interiorDeliveryZone.SetActive(false);
        }
    }
}
