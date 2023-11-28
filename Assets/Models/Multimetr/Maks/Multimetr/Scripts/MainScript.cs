using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public TextMesh tm;
    private rotateright rotateright;
    private rotateleft rotateleft;
    [SerializeField]
    private rotate rotate;
    [SerializeField]
    private button1 button1;
    [SerializeField]
    private button2 button2;
    [SerializeField]
    private ClickerMA clickerMA;
    [SerializeField]
    private Clicker10A clicker10A;
    [SerializeField]
    private ClickerV clickerV;
    [SerializeField]
    private ClickerCOM clickerCOM;
    [SerializeField]
    private WireBlack wireBlack;
    [SerializeField]
    private WireRed wireRed;
    [SerializeField]
    private BlackProbeScript blackProbe;
    [SerializeField]
    private RedProbeScript redProbe;
    [SerializeField]
    private GameObject EffectPrefab;
    private Vector3 EffectPos;
    // Start is called before the first frame update
    void Start()
    {
        tm = (TextMesh)GameObject.Find("TextMultimetr").GetComponent<TextMesh>();
        Debug.Log(tm.text);

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
    }
    private void OnOff()
    {
        if (button1.work1 == true)
        {
            tm.text = "000.00";
            Debug.Log("Я включился");
        }
        else
            tm.text = " ";
        //Debug.Log("Включение =" + button1.work1);
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
    }
    private void A10()
    {
    }
    private void COM()
    {
    }
    private void V()
    {

    }
    private void MA()
    {

    }
}
