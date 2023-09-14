using Assets.Scripts.Logic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject logPanel;
    public TMP_Text log_text;
    public TMP_Text time_Text;

    public Button btnShowPanel;
    public Button btnConnect;
    public Button btnLog;
    public Button btnQuit;

    public ModelsPanel modelsPanel;

    // Start is called before the first frame update
    void Start()
    {
        btnQuit.onClick.AddListener(() => { Application.Quit(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (time_Text != null)
        {
            if (ModelController.isConnected)
                time_Text.text = BaseUtils.TimeSecondsToString((int)BaseUtils.toFloat(Main.ModelController.getVar("curTime")));
            else
                time_Text.text = "Нет соединения";
        }
    }

    public void init()
    {
        logPanel.gameObject.SetActive(false);
        btnLog.onClick.AddListener(onShowLog);
        btnConnect.onClick.AddListener(Main.Instance.OnConnect);
        btnShowPanel.onClick.AddListener(() => { onShowPanel(!modelsPanel.gameObject.activeSelf); });
    }

    public void onShowPanel(bool state)
    {
        modelsPanel.gameObject.SetActive(state);
    }

    void onShowLog()
    {
        logPanel.gameObject.SetActive(!logPanel.gameObject.activeSelf);
    }
}
