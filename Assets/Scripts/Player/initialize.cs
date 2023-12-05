using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class initialize : MonoBehaviour
{
     private GameObject[] items;

    private void Start()
    {
        ButtonScript.laserPointer = (Laser)GameObject.Find("RightHand").GetComponent<Laser>();
        ButtonScript.hand = (Hand)GameObject.Find("RightHand").GetComponent<Hand>();
        items = GameObject.FindGameObjectsWithTag("Interact");
        foreach (var item in items)
        {
            item.GetComponent<ButtonScript>().Activete();
        }
    }
}
