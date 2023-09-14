using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class PPS_tren : MonoBehaviour
{
    private static PPS_tren _instance;
    public static PPS_tren Instance { get { return _instance; } }
    private List<string> logs = new List<string>();

    [DllImport("PPSDLL")]
    private static extern IntPtr createPPS(string pszProcId, string pszRoot, string pszTmpRoot);

    [DllImport("PPSDLL", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool SetFunc(IntPtr onAccept, IntPtr onCalc, IntPtr onLoad, IntPtr onSave);

    [DllImport("PPSDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern void calc(IntPtr instance);

    [DllImport("PPSDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern int getInVarIndex(string pszVarName);

    [DllImport("PPSDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern int getOutVarIndex(string pszVarName);

    [DllImport("PPSDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern float getInVarValue(int iVarNum);

    [DllImport("PPSDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern void setOutVarValue(int iVarNum, float vrValue);


    [DllImport("PPSDLL")]
    private static extern IntPtr freePPS(IntPtr instance);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void asuOnAccept(int type, int val);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void asuOnCalcStep(double step, double time);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int asuOnLoad(int type, [MarshalAs(UnmanagedType.LPStr)] string str);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int asuOnSave(int type, [MarshalAs(UnmanagedType.LPStr)] string str);

    asuOnAccept asuAccept = new asuOnAccept(onAcceptCallback);
    asuOnCalcStep asuCalcStep= new asuOnCalcStep(onCalcCallback);
    asuOnLoad asuLoad = new asuOnLoad(onLoadCallback);
    asuOnSave asuSave = new asuOnSave(onSaveCallback);

    public int indx1;
    public int indx2;
    public float val1 = 0;
    public float val2 = 0;

    private IntPtr pps;

    public float U1p = 24;
    public float U1m = 0;
    public float U2 = 0;
    public float U3 = 0;
    public float R1 = 10;
    public float R2 = 200;
    public float R3 = 200;
    public float R4 = 10;
    public float i1, i2, i3, i4;
    public float i0;
    public float dT = 0.01f;

    public float IO2;
    public float O2;

    public TMP_Text logText;
    public TMP_Text val1Text;
    public TMP_Text stepText;
    public TMP_Text timeText;

    void Awake()
    {

    }

    void Start()
    {
        //Application.targetFrameRate = 15;

        if (Instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        pps = createPPS("pmgate", "d:\\3D\\ppsdde", "d:\\3D\\ppsdde\\tmp");
        PPS_tren.Instance.showLog("Start = " + pps);

        IntPtr ptrAccept = Marshal.GetFunctionPointerForDelegate(asuAccept);
        IntPtr ptrCalc = Marshal.GetFunctionPointerForDelegate(asuCalcStep);
        IntPtr ptrLoad = Marshal.GetFunctionPointerForDelegate(asuLoad);
        IntPtr ptrSave = Marshal.GetFunctionPointerForDelegate(asuSave);
        bool a = SetFunc(ptrAccept, ptrCalc, ptrLoad, ptrSave);

        PPS_tren.Instance.showLog("GetFunctionPointerForDelegate = " + a);

        string var1 = "FRCA1";
        indx1 = getInVarIndex(var1);
        PPS_tren.Instance.showLog("getInVarIndex " + var1 + " indx =" + indx1);

        string var2 = "GATE1";
        indx2 = getOutVarIndex(var2);
        PPS_tren.Instance.showLog("getOutVarIndex " + var2 + " indx =" + indx2);

        Debug.Log(pps);
    }

    public static void onAcceptCallback(int type, int val)
    {
        //string result = Marshal.PtrToStringAnsi(ptr);
        //Debug.Log("Callback = " + val);
        PPS_tren.Instance.showLog("onAcceptCallback type="+ type +  " val=" + val);
    }

    public static void onCalcCallback(double step, double time)
    {
        //string result = Marshal.PtrToStringAnsi(ptr);
        //Debug.Log("Callback = " + val);
        Instance.val1 = getInVarValue(Instance.indx1);
        //Instance.val2 = Instance.meo1.ipv;
        setOutVarValue(Instance.indx2, Instance.val2);
        Instance.val1Text.text = Instance.val1.ToString();
        Instance.stepText.text = step.ToString();
        Instance.timeText.text = time.ToString();
        //PPS_tren.Instance.showLog("onCalcCallback step = " + step + " inVal = " + Instance.val1 + " outVal =" + Instance.val2);
    }

    public static int onLoadCallback(int type, [MarshalAs(UnmanagedType.LPStr)] string str)
    {
        //string result = Marshal.PtrToStringAnsi(ptr);
        //Debug.Log("Callback = " + val);
        //string state = String.Join("", str);
        PPS_tren.Instance.showLog("onLoadCallback type=" + type + " state=" + str);
        return 1;
    }

    public static int onSaveCallback(int type, [MarshalAs(UnmanagedType.LPStr)] string str)
    {
        //string result = Marshal.PtrToStringAnsi(ptr);
        //Debug.Log("Callback = " + val);
        //string state = String.Join("", str);
        PPS_tren.Instance.showLog("onSaveCallback type=" + type + " state=" + str);
        return 1;
    }

    void Update()
    {
        float i = -1;
        for (int k = 0; k < 100; k++)
        {
            i1 = getI(U1p, U2, R1);
            i2 = getI(U2, U3, R2, true);
            i3 = getI(U2, U3, R3, true);
            i4 = getI(U3, U1m, R4);

            if (U1p == 0) i1 = 0;
            if (U1p == 0) i4 = 0;

            //if ((R2 + R3) < 100000 || i0 > 0)
            U2 = U2 + (i1 - (i2 + i3 + IO2) - i0) * dT;

            //if (i1 == 0) U2 = 0;

            //if (R4 < 100000)
                U3 = U3 + (i2 + i3 + IO2 + i0 - i4) * dT;

            //if ((i2 + i3 + IO2) == 0) U3 = 0;

            //if (i == i1) break;
            i = i1;
        }

        O2 = (U2 - U3) / IO2;
    }

    float getI(float u1, float u2, float r, bool reverse = false)
    {
        var i = (u1 - u2) / (r + 0.1f);
        if (i > 100) i = 16;
        if (!reverse && i < 0) i = 0;
        return i;
    }

    private void OnDestroy()
    {
        freePPS(pps);
    }

    public void showLog(string mess)
    {
        if (logs.Count > 20) logs.RemoveRange(0, logs.Count - 20);
        logs.Add(mess);
        logText.text = "";

        foreach (string log in logs)
            logText.text += log + "\n\r";
    }
}
