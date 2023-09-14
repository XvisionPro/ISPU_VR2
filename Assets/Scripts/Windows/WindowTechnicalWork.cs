using System;
using Base;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class WindowTechnicalWork : CommonWindow
{
    public static void show(string pref = "WindowTechnicalWork")
    {
        if (CommonWindow.isWindowOpen(pref)) return;

        var winobj = AssetManager.getWindowPrefab(pref, Main.SystemWindowParent);
        var rectTransform = winobj.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.localScale = new Vector3(1, 1, 1);

        var thisWindow = winobj.GetComponent<WindowTechnicalWork>();
        thisWindow.name = pref;
        thisWindow.addBack(winobj);
                
        CommonWindow.WindowsArray.Add(thisWindow);

        thisWindow.onShow();
    }

    public override void init()
    {
        base.init();
        btn_ok.onClick.AddListener(onOk);
    }

    public override void onOk()
    {
        base.onOk();
    }
}
