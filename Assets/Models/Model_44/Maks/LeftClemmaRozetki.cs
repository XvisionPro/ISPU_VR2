using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftClemmaRozetki : MonoBehaviour
{
    private Action onChange;
    public bool ClickLeftClemmaRozetka = false;
    public Vector3 RightClemPos;
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
        ClickLeftClemmaRozetka = true;
        Debug.Log("Второй элемент нажат");
        if (onChange != null)
        {
            onChange();
        }
    }
    public void ClickLeftRozetka(Action on_change)
    {
        onChange = on_change;
    }
}
