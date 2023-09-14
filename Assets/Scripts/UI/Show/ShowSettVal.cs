using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowSettVal : MonoBehaviour
{
    public Button btnPlus;
    public Button btnMinus;
    public TMP_Text textVal;

    public float val;
    public float dVal;
    public float valMax;
    public float valMin;
    public float valShowKoef;

    public void init(float _val)
    {
        btnPlus.onClick.RemoveAllListeners();
        btnPlus.onClick.AddListener(() => { val += dVal;  refresh(); });

        btnMinus.onClick.RemoveAllListeners();
        btnMinus.onClick.AddListener(() => { val -= dVal; refresh(); });

        val = _val;

        refresh();
    }

    private void refresh()
    {
        if (val > valMax) val = valMax;
        if (val < valMin) val = valMin;

        textVal.text = (valShowKoef * val).ToString("0"); ;
    }
}
