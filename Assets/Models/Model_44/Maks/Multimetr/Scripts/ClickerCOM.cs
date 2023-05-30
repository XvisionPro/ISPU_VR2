using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerCOM : MonoBehaviour
{
    private Action onChange;
    public bool ClickCOM = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        ClickCOM = true;
        Debug.Log("Второй элемент нажат");
        if (onChange != null)
        {
            onChange();
        }
    }
    public void ButtonCOMClick(Action on_change)
    {
        onChange = on_change;
    }
}