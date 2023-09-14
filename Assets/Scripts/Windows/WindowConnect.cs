using System;
using System.Collections.Generic;
using Base;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowConnect : CommonWindow
{
    public TMP_InputField serverIPIn;
    public TMP_InputField serverPortIn;
    public TMP_InputField serverIPOut;
    public TMP_InputField serverPortOut;

    private Action onConnect;

    public static void show(Action _callback, string pref = "WindowConnect")
    {
        var winobj = AssetManager.getWindowPrefab(pref, Main.WindowParent);
        var rectTransform = winobj.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 1000, 0);
        rectTransform.localScale = new Vector3(1, 1, 1);

        var thisWindow = winobj.GetComponent<WindowConnect>();
        thisWindow.addBack(winobj);
        thisWindow.onConnect = _callback;

        CommonWindow.WindowsArray.Add(thisWindow);

        thisWindow.onShow(false);
    }

    public override void init()
    {
        base.init();

        btn_ok.onClick.AddListener(onOk);
        serverIPIn.text = Settings.localSett["serverIPIn"];
        serverPortIn.text = Settings.localSett["serverPortIn"];
        serverIPOut.text = Settings.localSett["serverIPOut"];
        serverPortOut.text = Settings.localSett["serverPortOut"];
    }

    public void onOk()
    {
        Main.Instance.network.InHostName = serverIPIn.text;
        Main.Instance.network.InPort = BaseUtils.toInt(serverPortIn.text);

        Main.Instance.network.OutHostName = serverIPOut.text;
        Main.Instance.network.OutPort = BaseUtils.toInt(serverPortOut.text);

        Settings.localSett["serverIPIn"] = serverIPIn.text;
        Settings.localSett["serverPortIn"] = serverPortIn.text;
        Settings.localSett["serverIPOut"] = serverIPOut.text;
        Settings.localSett["serverPortOut"] = serverPortOut.text;
        Settings.saveSett();
        Settings.setSettings();
        onClose();

        if (onConnect != null) onConnect();
    }
}
