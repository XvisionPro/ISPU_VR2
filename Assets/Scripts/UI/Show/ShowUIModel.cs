using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowUIModel : MonoBehaviour
{
    public Button btn;
    public Image img;
    public TMP_Text txt;
    
    private BaseModel model;
    private Action<BaseModel> onClick;

    void Start()
    {
        
    }

    public void init(BaseModel _model, Action<BaseModel> _onClick)
    {
        model = _model;
        onClick = _onClick;
        txt.text = model.baseName;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(()=> { onClick(model); });
        Main.AssetManager.getImgStreamingAssets("/Images/models/" + model.typeName, img);
        //Main.Instance.StartCoroutine(Main.AssetManager.loadImgstreamingAssets("/Images/models/" + model.typeName, img));
    }
}
