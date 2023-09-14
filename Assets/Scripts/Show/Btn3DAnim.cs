using Assets.Scripts.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn3DAnim : MonoBehaviour
{
    private string baseName = "";
    public string field;
    public float onVal;
    public float offVal;

    private float val = 0;

    private Action<float> Callback;
    public Animator Anim;

    void Start()
    {
    }

    public void init(string _baseName, Action<float> _callback = null)
    {
        baseName = _baseName;
        Callback = _callback;
    }

    void OnMouseDown()
    {
        if (val == onVal) 
            val = offVal;
        else
            val = onVal;

        if (Callback != null) Callback(val);
        
        Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + val);
    }

    void Update()
    {
        val = BaseUtils.toFloat(Main.ModelController.getVar(baseName + field));
        Anim.SetBool("state", val == onVal);
    }
}
