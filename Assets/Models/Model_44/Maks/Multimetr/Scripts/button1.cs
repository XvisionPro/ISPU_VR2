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
    public GameObject Self;
    public bool flag=true;
    // Start is called before the first frame update
    void Start()
    {
        basepos = Self.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trunik2()
    {
        if (work1 == false && flag==true)
        {
            needpos = new Vector3(gameObject.transform.localPosition.x + distance, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            Debug.Log(Self.transform.localPosition);
            Self.transform.localPosition = new Vector3(needpos.x, needpos.y, needpos.z);
            StartCoroutine(time(0.1f));
            Debug.Log(needpos);
            return;
        }
        else if (work1 == true && flag == true)
        {
            gameObject.transform.localPosition = basepos;
            StartCoroutine(time(0.1f));

        }
    }
    IEnumerator time(float time)
    {
        flag = false;
        yield return new WaitForSeconds(time);
        work1 = !work1;
        flag = true;

    }
    public void Button1Click(Action on_change)
    {
        onChange = on_change;
    }
}
