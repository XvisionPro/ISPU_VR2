using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaButtonUI : MonoBehaviour
{
    public bool clickmAButtonUI=false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMouseDown()
    {
        clickmAButtonUI = true;
    }
}
