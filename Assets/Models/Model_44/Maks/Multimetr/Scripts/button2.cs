using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button2 : MonoBehaviour
{
    private Action onChange;
    private Vector3 basepos;
    private Vector3 needpos;
    public float distance = -0.3f;
    public bool work2 = false;
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
        if (work2 == false)
        {
            needpos = new Vector3(gameObject.transform.localPosition.x + distance, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            gameObject.transform.localPosition = needpos;
            work2 = true;
        }
        else if (work2 == true)
        {
            gameObject.transform.localPosition = basepos;
            work2 = false;
        }
        if (onChange != null)
        {
            onChange();
        }
    }
    public void Button2Click(Action on_change)
    {
        onChange = on_change;
    }
}
