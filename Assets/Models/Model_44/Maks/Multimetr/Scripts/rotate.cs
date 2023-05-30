using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private Action onChange;
    private GameObject krytilka;
    private Vector3 needpos;
    public float distance = 15;
    private bool work = false;
    public int counter=0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Rotationright()
    {
        this.transform.Rotate(0.0f, 15.0f, 0.0f, Space.Self);
        counter++;
        if (counter==24)
        {
            counter = 0;
        }
        if (onChange!=null)
        {
            onChange();
        }
        //Debug.Log("Сейчас режим"+counter);

    }
    public void Rotationleft()
    {
        this.transform.Rotate(0.0f, -15.0f, 0.0f, Space.Self);
        if (counter <= 0)
        {
            counter++;
            counter = 24 - counter;
        }
        else counter--;
        if (onChange != null)
        {
            onChange();
        }
        //Debug.Log("Сейчас режим" + counter);
    }
    public void Rotating(Action on_change)
    {
        onChange = on_change;
    }
}
