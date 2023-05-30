using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button1 : MonoBehaviour
{
    private Action onChange;
    private Vector3 basepos;
    private Vector3 needpos;
    public float distance = -0.3f;
    public bool work1 = false;
    // Start is called before the first frame update
    void Start()
    {
        basepos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        if (work1 == false)
        {
            needpos = new Vector3(gameObject.transform.localPosition.x + distance, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            gameObject.transform.localPosition = needpos;
            work1 = true;
        }
        else if (work1==true)
        {
            gameObject.transform.localPosition = basepos;
            work1 = false;
        }
        if (onChange != null)
        {
            onChange();
        }
    }
    public void Button1Click(Action on_change)
    {
        onChange = on_change;
    }
}
