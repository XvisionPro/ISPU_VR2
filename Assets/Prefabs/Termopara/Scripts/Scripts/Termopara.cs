using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.MPE;
using UnityEngine;

public class Termopara : MonoBehaviour
{

    [SerializeField] AnimProvod provodLeft; // в данном классе получаем событие левого провода 
    [SerializeField] AnimProvod provodRight; // в данном классе получаем событие правого провода  
    private float T2 = 25f; // температура измерения
    private float T2r; // выводим темперутуру спава на табло
    private float T1 = 25f; // комнатная температура
    private float deltaT;
    private float EdsLeft;
    private bool flagOpen = false;

    void OnClick()
    {
        if (flagOpen)
            flagOpen = false;
        else flagOpen = true;
    }
    
}
