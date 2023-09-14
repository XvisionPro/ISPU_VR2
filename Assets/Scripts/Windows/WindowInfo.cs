using System;
using Base;
using UnityEngine;
using UnityEngine.UI;

public class WindowInfo: CommonWindow
{
    [SerializeField]
    public Text header;
    public Text message;
    public Text message2;
    public Text btn;
    public Text btn_no;
    public Image image;
    public Image imageback;

    private Action<bool> callback;
    private string header_text;
    private string mess_text;
    private string mess2_text;
    private string btn_text;
    private string btn_no_text;

    /*public static void showTransl(string h_text, string m_text, string b_text, Action<bool> callback = null, string pref = "WindowInfo", string imgback = "", string img = "")
    {
        show(Translate.getText(h_text), Translate.getText(m_text), "", Translate.getText(b_text), callback, pref, imgback, img);
    }
    public static void showTransl(string h_text, string m_text, string m2_text, string b_text, Action<bool> callback = null, string pref = "WindowInfo", string imgback = "", string img = "")
    {
        show(Translate.getText(h_text), Translate.getText(m_text), Translate.getText(m2_text), Translate.getText(b_text), callback, pref, imgback, img);
    }

    public static void showTransl(string h_text, string m_text, string m2_text, string yes_text, string not_text, Action<bool> yesback = null, Action noback = null, string pref = "WindowInfo")
    {
        var thisWindow = loadWindow(pref);

        CommonWindow.WindowsArray.Add(thisWindow);
        thisWindow.callback = yesback;
        thisWindow.cancel_callback = noback;
        thisWindow.header_text = Translate.getText(h_text);
        thisWindow.mess_text = Translate.getText(m_text);
        thisWindow.mess2_text = Translate.getText(m2_text);
        thisWindow.btn_text = Translate.getText(yes_text);
        thisWindow.btn_no_text = Translate.getText(not_text);

        thisWindow.onShow();
    }*/

    public static void showNoTransl(string h_text, string m_text, string yes_text, string not_text, Action<bool> yesback = null, Action noback = null, string pref = "WindowInfo")
    {
        var thisWindow = loadWindow(pref);

        CommonWindow.WindowsArray.Add(thisWindow);
        thisWindow.callback = yesback;
        thisWindow.cancel_callback = noback;
        thisWindow.header_text = h_text;
        thisWindow.mess_text = m_text;
        thisWindow.btn_text = yes_text;
        thisWindow.btn_no_text = not_text;

        thisWindow.onShow();
    }

    public static void showInfo(string h_text, string m_text, string yes_text, string pref = "WindowInfo")
    {
        var thisWindow = loadWindow(pref);

        CommonWindow.WindowsArray.Add(thisWindow);
        thisWindow.header_text = h_text;
        thisWindow.mess_text = m_text;
        thisWindow.btn_text = yes_text;
        if (thisWindow.btn_cancel) thisWindow.btn_cancel.gameObject.SetActive(false);
        thisWindow.onShow();
    }

    public static CommonWindow show(string h_text, string m_text, string m2_text, string yes_text, string no_text, Action<bool> callback = null, string pref = "WindowInfo", string imgback = "", string img = "")
    {
        var thisWindow = loadWindow(pref);

        CommonWindow.WindowsArray.Add(thisWindow);
        thisWindow.callback = callback;
        thisWindow.header_text = h_text;
        thisWindow.mess_text = m_text;
        thisWindow.mess2_text = m2_text;
        thisWindow.btn_text = yes_text;
        thisWindow.btn_no_text= no_text;

        if (img != "" && thisWindow.image != null)
            thisWindow.image.sprite = Main.AssetManager.getSpriteFromBundle(img);

        if (imgback != "" && thisWindow.imageback != null)
            thisWindow.imageback.sprite = Main.AssetManager.getSpriteFromBundle(imgback);

        thisWindow.onShow();

        return thisWindow;
    }

    private static WindowInfo loadWindow(string pref)
    {
        GameObject parent = GameObject.Find("WindowsCanvas");
        GameObject winobj = AssetManager.getWindowPrefab(pref, parent.transform);
        
        var rectTransform = winobj.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 1000, 0);
        rectTransform.localScale = new Vector3(1, 1, 1);

        var thisWindow = winobj.GetComponent<WindowInfo>();
        thisWindow.addBack(winobj);
        thisWindow.name = pref;

        return thisWindow;
    }

    public override void init()
    {
        base.init();
        btn_ok.onClick.AddListener(onOk);
        keyEnterAction = onOk;
        keyEscAction = onClose;

        if (header != null) header.text = header_text;
        if (message != null) message.text = mess_text;
        if (message2 != null) message2.text = mess2_text;
        if (btn != null) btn.text = btn_text;
        if (btn_no != null) btn_no.text = btn_no_text;
        if (btn_no_text == "") btn_no.gameObject.SetActive(false);
    }

    public override void onClose()
    {
        if (!windowShowed) return;

        base.onClose();

        if (callback != null)
        {
            callback(false);
        }
    }

    public void onOk()
    {
        close_callback = null;
        if (callback != null)
        {
            callback(true);
            callback = null;
        }

        onClose();
    }
}
