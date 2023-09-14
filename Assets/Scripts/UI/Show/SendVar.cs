using Assets.Scripts.Logic;
using UnityEngine;
using UnityEngine.UI;

public class SendVar : MonoBehaviour
{
    public Button btn;
    public string par;
    public float val;

    void Start()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(onSend);
    }

    void onSend()
    {
        if (Main.Instance != null) 
            Main.Instance.network.send(AllTypes.MESS_DATA + ":" + par + "=" + val);
    }
}