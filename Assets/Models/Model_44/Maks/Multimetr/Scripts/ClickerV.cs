using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerV : MonoBehaviour
{
    private Action onChange;
    public bool ClickV = false;
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
        ClickV = true;
        Debug.Log("Второй элемент нажат");
        if (onChange != null)
        {
            onChange();
        }
    }
    public void ButtonVClick(Action on_change)
    {
        onChange = on_change;
    }
}