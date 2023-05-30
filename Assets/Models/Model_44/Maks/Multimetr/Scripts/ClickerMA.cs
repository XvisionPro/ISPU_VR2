using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerMA : MonoBehaviour
{
    private Action onChange;
    public bool ClickMA=false;
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
        ClickMA = true;
        Debug.Log("Второй элемент нажат");
        if (onChange != null)
        {
            onChange();
        }
    }
    public void ButtonMACLick(Action on_change)
    {
        onChange = on_change;
    }
}
