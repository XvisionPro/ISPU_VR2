using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.MPE;
using UnityEngine;

public class Termopara : MonoBehaviour
{

    [SerializeField] AnimProvod provodLeft; // � ������ ������ �������� ������� ������ ������� 
    [SerializeField] AnimProvod provodRight; // � ������ ������ �������� ������� ������� �������  
    private float T2 = 25f; // ����������� ���������
    private float T2r; // ������� ����������� ����� �� �����
    private float T1 = 25f; // ��������� �����������
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
