using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CommonWindow : MonoBehaviour
{
    public static List<CommonWindow> WindowsQuere = new List<CommonWindow>();
    public static List<CommonWindow> WindowsArray = new List<CommonWindow>();
    public static bool isLocked = false;
    public static CommonWindow curentOpenedWindow;

    [SerializeField]
    public GameObject content;
    [SerializeField]
    public Button btn_close;
    [SerializeField]
    public Button btn_ok;
    [SerializeField]
    public Button btn_cancel;

    public bool needSound = false;
    public float closetimer = 0.5f;
    public Action keyEnterAction = null;
    public Action keyEscAction = null;

    protected Action ok_callback;
    protected Action close_callback;
    protected Action cancel_callback;

    protected GameObject backobj;

    protected bool windowShowed = false;
    private bool _needQuere = false;

    protected Dictionary<string, GameObject> contentDictionary = new Dictionary<string, GameObject>();

    public static void CloseAllWindows()
    {
        var cnt = WindowsArray.Count;
        for (var i = cnt - 1; i >=0; i--)
        {
            if (WindowsArray[i] != null)
            {
                if (WindowsArray[i].name != "WindowErr") WindowsArray[i].onClose();
            }
            else
                WindowsArray.RemoveAt(i);

        }
    }

    public static void CheckNextWindows()
    {
        try
        {
            if (WindowsQuere.Count == 0 || IsAnyWindowOpen() || !IsQueueAvailable()) return;
            if (WindowsQuere[0].content != null) WindowsQuere[0].content.SetActive(true);
            WindowsQuere[0].onShow(WindowsQuere[0]._needQuere);
        }
        catch
        {
            Debug.Log("Error CheckNextWindows");
        }
    }

    public static bool IsQueueAvailable()
    {
        WindowsArray.RemoveAll(x => x == null);
        WindowsQuere.RemoveAll(x => x == null);

        return WindowsArray.All(window => window._needQuere);
    }

    public static bool IsAnyWindowOpen()
    {
        WindowsArray.RemoveAll(x => x == null);
        WindowsQuere.RemoveAll(x => x == null);

        return WindowsArray.Any(window => window.gameObject.activeSelf);
    }

    public static CommonWindow show(string prefName, bool needBack, Action callback)
    {
        GameObject winobj = AssetManager.getPrefab("Windows/" + prefName, Main.WindowParent);

        var rectTransform = winobj.GetComponent<RectTransform>();

        if (winobj.GetComponent<EasyTween>() != null)
            rectTransform.localPosition = new Vector3(0, 1000, 0);
        else
            rectTransform.localPosition = new Vector3(0, 0, 0);

        rectTransform.localScale = new Vector3(1, 1, 1);

        var thisWindow = winobj.GetComponent<CommonWindow>();

        if (needBack)
            thisWindow.addBack(winobj);

        WindowsArray.Add(thisWindow);
        thisWindow.close_callback = callback;

        thisWindow.onShow();

        return thisWindow;
    }

    // Use this for initialization
    void Start () {
        init();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (keyEnterAction != null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                keyEnterAction();
            }
        }
        
        if (keyEscAction!= null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                keyEscAction();
            }
        }
    }

    public void addBack(GameObject winobj, bool hud = false)
    {
        backobj = AssetManager.getWindowPrefab("LockBack");
        
        if (hud && Main.Hud != null)
            backobj.GetComponent<RectTransform>().SetParent(Main.Instance.hud.transform);
        else
            backobj.GetComponent<RectTransform>().SetParent(winobj.transform);

        backobj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        backobj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        backobj.transform.SetAsFirstSibling();
    }

    public void clickdBack(Action callback)
    {
        backobj.GetComponent<LockBack>().callback = callback;
    }

    public virtual void init()
    {
        if (btn_close !=null) btn_close.onClick.AddListener(onClose);
        if (btn_cancel != null) btn_cancel.onClick.AddListener(onCancel);

//        foreach (Transform child in transform)
//            getAllObjectsInTransform(child);

        contentDictionary = ExternalScripts.Utils.getAllObjectsInTransform(transform);
    }

    public void onShow(bool needQuere = false, bool firstInQuere = false) 
    {
        _needQuere = needQuere;

        if (curentOpenedWindow != null && needQuere)
        {
            content.SetActive(false);
            if (firstInQuere)
                WindowsQuere.Insert(0, this);
            else 
                WindowsQuere.Add(this);
            return;
        }

        curentOpenedWindow = this;

        if (content.GetComponent<EasyTween>() != null)
        {
            content.GetComponent<EasyTween>().OpenCloseObjectAnimation();
        }

        isLocked = true;
        content.SetActive(true);
        Invoke("onShowed", 0.2f);

        openSound();
        
        //EventController.dispatchEventWith(UserEvents.WINDOW_SHOW, this);
    }

    public virtual void openSound()
    {
        //if (needSound) Main.soundController.playSound(Main.soundController.audioList.winOpen);
    }

    public virtual void onShowed()
    {
        windowShowed = true;
    }

    public virtual void onCancel()
    {
        if (!windowShowed) return;

        if (close_callback != null)
        {
            onClose();
            return;
        }

        if (btn_cancel != null) btn_cancel.onClick.RemoveAllListeners();

        if (content != null && content.GetComponent<EasyTween>() != null && content.GetComponent<EasyTween>().IsObjectOpened())
            content.GetComponent<EasyTween>().OpenCloseObjectAnimation();

        WindowsArray.Remove(this);
        WindowsQuere.Remove(this);

        Main.Instance.Delay(closetimer,
        delegate
        {
            Destroy(backobj);
            Destroy(content);
            Destroy(this);
        });

        if (WindowsArray.Count == 0) isLocked = false;
        if (cancel_callback != null)
        {
            cancel_callback();
        }

        curentOpenedWindow = null;
        
        CheckNextWindows();
    }

    public virtual void onClose()
    {
        if (!windowShowed) return;

        if (btn_close != null) btn_close.onClick.RemoveAllListeners();
        if (btn_cancel != null) btn_cancel.onClick.RemoveAllListeners();
        if (btn_ok != null) btn_ok.onClick.RemoveAllListeners();

        if (content != null && content.GetComponent<EasyTween>() != null && content.GetComponent<EasyTween>().IsObjectOpened())
            content.GetComponent<EasyTween>().OpenCloseObjectAnimation();

        WindowsArray.Remove(this);
        WindowsQuere.Remove(this);

        Main.Instance.Delay(closetimer,
            delegate
            {
                Destroy(backobj);
                Destroy(content);
                Destroy(this);
                content = null;
            });

        if (WindowsArray.Count == 0) isLocked = false;
        if (close_callback != null)
        {
            close_callback();
            close_callback = null;
        }
        
        //EventController.dispatchEventWith(UserEvents.WINDOW_CLOSE, this);

        curentOpenedWindow = null;

        CheckNextWindows();
    }

    public virtual void onOk()
    {
        if (!windowShowed) return;

        if (btn_ok != null) btn_ok.onClick.RemoveAllListeners();

        if (content != null && content.GetComponent<EasyTween>() != null && content.GetComponent<EasyTween>().IsObjectOpened())
            content.GetComponent<EasyTween>().OpenCloseObjectAnimation();

        WindowsArray.Remove(this);
        WindowsQuere.Remove(this);

        Main.Instance.Delay(closetimer,
        delegate
        {
            Destroy(backobj);
            Destroy(content);
            Destroy(this);
        });

        if (WindowsArray.Count == 0) isLocked = false;
        if (ok_callback != null)
        {
            ok_callback();
        }

        curentOpenedWindow = null;

        CheckNextWindows();
    }

    public static bool isWindowOpen(string name)
    {
        WindowsArray.RemoveAll(x => x == null);
        WindowsQuere.RemoveAll(x => x == null);

        foreach (CommonWindow win in WindowsArray)
        {
            if (win != null && win.name == name)
            {
                return true;
            }
        }

        return false;
    }

    public static void closeIfOpen(string name)
    {
        WindowsArray.RemoveAll(x => x == null);
        WindowsQuere.RemoveAll(x => x == null);

        foreach (CommonWindow win in WindowsArray)
        {
            if (win != null && win.name == name)
            {
                WindowsArray.Remove(win);
                WindowsQuere.Remove(win);
                Destroy(win.backobj);
                Destroy(win.content);
                Destroy(win);

                if (curentOpenedWindow = win)
                    curentOpenedWindow = null;
                return;
            }
        }
    }

    public static bool IsWindowInQueue(string name)
    {
        return WindowsQuere.Any(window => window.name == name);
    }
}
