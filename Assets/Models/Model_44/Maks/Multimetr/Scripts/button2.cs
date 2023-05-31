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
    public bool flag = true;
    // Start is called before the first frame update
    void Start()
    {
        basepos = gameObject.transform.localPosition;
    }

    // Update is called once per frame

    public void Click()
    {
        if (work2 == false && flag == true)
        {
            needpos = new Vector3(gameObject.transform.localPosition.x + distance, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            gameObject.transform.localPosition = needpos;
            StartCoroutine(time(0.1f));
        }
        else if (work2 == true && flag == true)
        {
            gameObject.transform.localPosition = basepos;
            StartCoroutine(time(0.1f));
        }
        if (onChange != null)
        {
            onChange();
        }
    }
    IEnumerator time(float time)
    {
        flag = false;
        yield return new WaitForSeconds(time);
        work2 = !work2;
        flag = true;

    }
    public void Button2Click(Action on_change)
    {
        onChange = on_change;
    }
}
