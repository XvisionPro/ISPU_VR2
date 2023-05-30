using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClemmaPribora : MonoBehaviour
{
    private Action onChange;
    public bool ClickRightClemma = false;
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
        ClickRightClemma = true;
        Debug.Log("Второй элемент нажат");
        if (onChange != null)
        {
            onChange();
        }
        RightClemPos = gameObject.transform.localPosition;
    }
    public void ClickRightclemma(Action on_change)
    {
        onChange = on_change;
    }
}
