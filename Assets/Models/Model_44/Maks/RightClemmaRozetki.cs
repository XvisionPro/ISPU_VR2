using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClemmaRozetki : MonoBehaviour
{
    private Action onChange;
    public bool ClickRightClemmaRozetka = false;
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
        ClickRightClemmaRozetka = true;
        Debug.Log("Второй элемент нажат");
        if (onChange != null)
        {
            onChange();
        }
    }
    public void ClickRightRozetka(Action on_change)
    {
        onChange = on_change;
    }
}
