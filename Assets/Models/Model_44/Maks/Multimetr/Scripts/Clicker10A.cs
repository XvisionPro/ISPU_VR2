using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clicker10A : MonoBehaviour
{
    private Action onChange;
    public bool Click10A = false;
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
        Click10A = true;
        Debug.Log("Третий элемент нажат");
        if (onChange != null)
        {
            onChange();
        }
    }
    public void Button10AClick(Action on_change)
    {
        onChange = on_change;
    }

}
