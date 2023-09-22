using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLocation : MonoBehaviour
{
    public List<BaseModel> pribors;

    public void Awake()
    {
        foreach (BaseModel model in pribors)
        {
            model.gameObject.SetActive(true);
        }
    }

    public void init()
    {
        foreach (BaseModel model in pribors)
        {
            model.gameObject.SetActive(true);
            model.setDefault();
        }

        Main.Hud.onShowPanel(false);
        Main.ModelController.ClearMesure();
        //Main.Instance.Player.SetActive(true);
    }
}
