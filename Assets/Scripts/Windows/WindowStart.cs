using System;
using System.Collections.Generic;
using Base;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowStart : CommonWindow
{
    public Button btnStart;
    public Button btnStartVR;

    public static void show(string pref = "WindowStart")
    {
        var winobj = AssetManager.getWindowPrefab(pref, Main.WindowParent);
        var rectTransform = winobj.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 1000, 0);
        rectTransform.localScale = new Vector3(1, 1, 1);

        var thisWindow = winobj.GetComponent<WindowStart>();
        thisWindow.addBack(winobj);

        WindowsArray.Add(thisWindow);

        thisWindow.onShow(false);
    }

    public override void init()
    {
        base.init();

        btnStart.onClick.RemoveAllListeners();
        btnStart.onClick.AddListener(() =>
        {
            Main.Instance.createScene(Main.Instance.Objects);
            var gameObject = GameObject.Find("Menu");
            gameObject.SetActive(false);
            onClose();
        });

        btnStartVR.onClick.RemoveAllListeners();
        btnStartVR.onClick.AddListener(() =>
        {
            Main.Instance.createScene(Main.Instance.VRObjects);
            var gameObject = GameObject.Find("Menu");
            gameObject.SetActive(false);
            onClose();
        });

    }
}
