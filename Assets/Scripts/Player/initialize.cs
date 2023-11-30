using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class initialize : MonoBehaviour
{
    public ButtonScript item;

    private void Start()
    {
        item = (ButtonScript)GameObject.Find("Cube228").GetComponent<ButtonScript>();
        item.Activete();
    }
}
