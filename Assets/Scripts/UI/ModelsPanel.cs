using Assets.Scripts.Logic;
using Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsPanel : MonoBehaviour
{
    public GameObject modelsContainer;
    private Dictionary<string, ShowUIModel> showModelList;

    void Start()
    {
        
    }

    void Update()
    {
        if (ModelController.modelNeedUpdate) show();
    }

    public void show()
    {
        BaseUtils.RemoveAllChildren(modelsContainer.transform);
        showModelList = new Dictionary<string, ShowUIModel>();

        foreach (var mdl in ModelController.AllModels)
        {
            addNewModelUI(mdl.Value);
        }

        ModelController.modelNeedUpdate = false;
    }

    public ShowUIModel addNewModelUI(BaseModel mdl)
    {
        var mdlObj = AssetManager.getPrefab("UI/showUIModel", modelsContainer.transform);

        var showModel = mdlObj.GetComponent<ShowUIModel>();
        showModel.init(mdl, onModelClick);
        showModelList.Add(mdl.baseName, showModel);
        return showModel;
    }

    private void onModelClick(BaseModel mdl)
    {
        Main.Instance.location.init();
        //Main.ModelController.showModel(mdl);
    }
}
