using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WireBlack : MonoBehaviour
{
    private Vector3 basepos;
    private Vector3 needpos;
    private Vector3 posemA;
    public Transform mA;
    private Vector3 mA_Position;
    public Transform A10;
    private Vector3 A10_Position;
    private bool Click1 = false;
    [SerializeField]
    private ClickerMA clickerMA;
    [SerializeField]
    private Clicker10A clicker10A;
    [SerializeField]
    private ClickerCOM clickerCOM;
    [SerializeField]
    private ClickerV clickerV;
    [SerializeField]
    private BlackButtonUI blackButtonUI;
    [SerializeField]
    private MaButtonUI maButtonClick;
    [SerializeField]
    private A10ButtonUI a10ButtonUI;
    [SerializeField]
    private COMButtomUI cOMButtomUI;
    [SerializeField]
    private VButtonUI vButtonUI;
    public GameObject gameObject1;
    public bool WireBlackOn = false;
    [SerializeField]
    private Animation anima;
    public bool ConnectTomA=false;
    public bool ConnectTo10A=false;
    public bool ConnectToCOM = false;
    public bool ConnectToV = false;
    // Start is called before the first frame update
    void Start()
    {
        //anim = gameObject.GetComponent<Animator>();
        if (mA == null)
        {
            mA = GameObject.Find("mA").GetComponent<Transform>();
        }
        basepos = gameObject.transform.localPosition;
        if (A10 == null)
        {
            A10 = GameObject.Find("10A").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((clickerMA.ClickMA || maButtonClick.clickmAButtonUI) && (Click1 || blackButtonUI.clickBlackButton))
        {
            ConnectTomA = true;
            anima.Play("WireStartMa");
            Click1 = false;
            clickerMA.ClickMA = false;
            blackButtonUI.clickBlackButton = false;
            maButtonClick.clickmAButtonUI = false;
        }
        else if ((clicker10A.Click10A || a10ButtonUI.click10AButton) && (Click1 || blackButtonUI.clickBlackButton))
        {
            ConnectTo10A = true;
            anima.Play("WireStart10A");
            Click1 = false;
            clicker10A.Click10A = false;
            a10ButtonUI.click10AButton = false;
            blackButtonUI.clickBlackButton = false;
        }
        else if ((clickerCOM.ClickCOM || cOMButtomUI.clickCOMButton) && (Click1 || blackButtonUI.clickBlackButton))
        {
            ConnectToCOM = true;
            WireBlackOn = true;
            anima.Play("WireStartCOM");
            Click1 = false;
            clickerCOM.ClickCOM = false;
            cOMButtomUI.clickCOMButton = false;
            blackButtonUI.clickBlackButton = false;
        }
        else if ((clickerV.ClickV || vButtonUI.clickVButton) && (Click1 || blackButtonUI.clickBlackButton))
        {
            ConnectToV = true;
            anima.Play("WireStartV");
            Click1 = false;
            clickerV.ClickV = false;
            vButtonUI.clickVButton = false;
            blackButtonUI.clickBlackButton = false;
        }
        else if ((Click1 || maButtonClick.clickmAButtonUI) && gameObject.transform.localPosition==new Vector3(1.53f, 0.96f, 4.8f))
        {
            ConnectTomA = false;
            anima.Play("WireBackMa");
            Click1 = false;
        }
        else if ((Click1 || a10ButtonUI.click10AButton) && gameObject.transform.localPosition == new Vector3(1.548f, 1.07f, 6.26f))
        {
            ConnectTo10A = false;
            anima.Play("WireBack10A");
            Click1 = false;
        }
        else if ((Click1 || cOMButtomUI.clickCOMButton) && gameObject.transform.localPosition == new Vector3(-0.248f, 1.02f, 5.591f))
        {
            ConnectToCOM = false;
            anima.Play("WireBackCOM");
            WireBlackOn = false;
            Click1 = false;
        }
        else if ((Click1 || vButtonUI.clickVButton) && gameObject.transform.localPosition == new Vector3(-2.178f, 0.932f, 5.535f))
        {
            ConnectToV = false;
            anima.Play("WireBackV");
            Click1 = false;
        }
    }
    private void OnMouseDown()
    {
        mA_Position = mA.localPosition;
        A10_Position = A10.localPosition;

        Click1 = true;
        Debug.Log("Первый элемент нажат");
    }
}
