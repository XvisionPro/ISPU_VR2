using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript_1 : MonoBehaviour
{
    public TextMesh tm;
    private rotateright_1 rotateright;
    private rotateleft_1 rotateleft;
    [SerializeField]
    private rotate_1 rotate;
    [SerializeField]
    private button1_1 button1;
    [SerializeField]
    private button2_1 button2;
    [SerializeField]
    private ClickerMA_1 clickerMA;
    [SerializeField]
    private Clicker10A_1 clicker10A;
    [SerializeField]
    private ClickerV_1 clickerV;
    [SerializeField]
    private ClickerCOM_1 clickerCOM;
    [SerializeField]
    private WireBlack_1 wireBlack;
    [SerializeField]
    private WireRed_1 wireRed;
    [SerializeField]
    private BlackProbeScript1 blackProbe;
    [SerializeField]
    private RedProbeScript_1 redProbe;
    [SerializeField]
    private GameObject EffectPrefab;
    private Vector3 EffectPos;
    // Start is called before the first frame update
    void Start()
    {
        tm = (TextMesh)GameObject.Find("TextMultimetr").GetComponent<TextMesh>();
        rotate.Rotating(KrytilkaChanged);
        rotate.Rotating(OnOff);
        button1.Button1Click(OnOff);
        button2.Button2Click(AC_DC);
        clickerMA.ButtonMACLick(MA);
        clicker10A.Button10AClick(A10);
        clickerV.ButtonVClick(V);
        clickerCOM.ButtonCOMClick(COM);
        redProbe.RedProbeVoid(OnOff);
        blackProbe.BlackProbeVoid(OnOff);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void KrytilkaChanged()
    {
        Debug.Log("Сейчас режим" + rotate.counter);
    }
    private void OnOff()
    {
        if (button1.work1 == true)
            tm.text = "000.00";
        else
            tm.text = " ";
        Debug.Log("Включение =" + button1.work1);
        if (button1.work1 == true && (blackProbe.ConnectToLeftClemma||blackProbe.ConnectToRightClemma)&&(redProbe.ConnectToRightClemma || redProbe.ConnectToLeftClemma) && (rotate.counter==2) && (wireBlack.ConnectToCOM && wireRed.ConnectToV))
        {
            tm.text = "000024";
        }
        if (button1.work1 == true && button2.work2 == true && (blackProbe.ConnectToLeftClemma || blackProbe.ConnectToRightClemma) && (redProbe.ConnectToRightClemma || redProbe.ConnectToLeftClemma) && (rotate.counter == 22) && (wireBlack.ConnectToCOM && wireRed.ConnectTo10A))
        {
            tm.text = "0002.0";
        }
        if (button1.work1 == true && button2.work2 == false && (blackProbe.ConnectToLeftClemmaRozetki || blackProbe.ConnectToRightClemmaRozetki) && (redProbe.ConnectToRightClemmaRozetki || redProbe.ConnectToLeftClemmaRozetki) && (rotate.counter == 1) && (wireBlack.ConnectToCOM && wireRed.ConnectToV))
        {
            tm.text = ("000" + Random.Range(218, 224));
        }
        if (button1.work1 == true && button2.work2 == false && (blackProbe.ConnectToLeftClemmaRozetki || blackProbe.ConnectToRightClemmaRozetki) && (redProbe.ConnectToRightClemmaRozetki || redProbe.ConnectToLeftClemmaRozetki) && (rotate.counter == 2 || rotate.counter == 3 || rotate.counter == 4 || rotate.counter == 5) && (wireBlack.ConnectToCOM && wireRed.ConnectToV))
        {
            tm.text = ("000001");
        }
        if (button1.work1 == true && button2.work2 == false && (blackProbe.ConnectToLeftClemmaRozetki || blackProbe.ConnectToRightClemmaRozetki) && (redProbe.ConnectToRightClemmaRozetki || redProbe.ConnectToLeftClemmaRozetki) && (rotate.counter == 22 || rotate.counter == 21) && (wireBlack.ConnectToCOM && wireRed.ConnectTo10A))
        {
            Vector3 EffectPos= new Vector3(-12.44f, 3.27f, 6.28f);
            Quaternion quaternion = Quaternion.Euler(0, 0, 0);
            Instantiate(EffectPrefab, EffectPos,quaternion);
        }
    }
    private void AC_DC()
    {
        Debug.Log("Режим AC/DC" + button2.work2);
    }
    private void A10()
    {
        Debug.Log("Подключен коннектор к 10A");
    }
    private void COM()
    {
         Debug.Log("Подключен коннектор к COM");
    }
    private void V()
    {
        Debug.Log("Подключен коннектор к V");
    }
    private void MA()
    {
        Debug.Log("Подключен коннектор к mA");
    }
}
