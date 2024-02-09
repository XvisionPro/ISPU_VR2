using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OnOff : MonoBehaviour
{
    public GameObject hint;
    public bool flag;

    private void Start()
    {
        flag = true;
    }

    public void Click()
    {

        if (flag == true)
        {
            hint.GetComponent<VideoPlayer>().Play();    
            flag = false;
            return;
        }
        if (flag == false)
        {
            hint.GetComponent<VideoPlayer>().Stop();
            flag = true;
            return;
        }
    }
}
