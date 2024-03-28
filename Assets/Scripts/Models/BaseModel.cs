using Assets.Scripts.Logic;
using Assets.Scripts.Models;
using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BaseModel : MonoBehaviour
{
    public static float obrivU = -1; //Напряжение в точки обрыва, не земля
    protected Dictionary<string, string> valR;
    protected Dictionary<string, string> valU;

    public string baseName;
    public string typeName;
    public List<Location> locations;

    [SerializeField]
    private Button btnSheme;
    [SerializeField]
    private GameObject sheme;

    private int curLocation = -1;

    public static BaseModel init(string _baseName, string _typename, Transform parent)
    {
        var openModel = ModelController.isModelOpen(_baseName);
        if (openModel != null)
        {
            return openModel;
        }
        else
        {
            var winobj = AssetManager.getPrefab("Models/" + _typename, parent);

            var thisModel = winobj.GetComponent<BaseModel>();
            thisModel.baseName = _baseName;
            thisModel.typeName = _typename;

            ModelController.ShowedModels.Add(thisModel);
            thisModel.setDefault();

            return thisModel;
        }
    }

    public virtual void Start()
    {
        btnSheme.onClick.RemoveAllListeners();
        btnSheme.onClick.AddListener(() => {
            sheme.SetActive(!sheme.activeSelf);
        });

        sheme.SetActive(false);
        setDefault();
    }

    public void initBaseChild()
    {
        List<GameObject> listOfChildren = new List<GameObject>();
        void GetChildRecursive(GameObject obj)
        {
            if (null == obj)
                return;

            foreach (Transform child in obj.transform)
            {
                if (null == child) continue;
                listOfChildren.Add(child.gameObject);
                GetChildRecursive(child.gameObject);
            }
        }

        GetChildRecursive(this.gameObject);

        for (int i = 0; i < listOfChildren.Count; i++)
        {
            btnToggle childToggle = listOfChildren[i].GetComponent<btnToggle>();
            if (childToggle != null) childToggle.init(baseName);

            ShowLamp childLamp = listOfChildren[i].GetComponent<ShowLamp>();
            if (childLamp != null) childLamp.init(baseName);
            
            ShowVar childVar = listOfChildren[i].GetComponent<ShowVar>();
            if (childVar != null) childVar.init(baseName);

            Btn3DAnim childbtn3D = listOfChildren[i].GetComponent<Btn3DAnim>();
            if (childbtn3D != null) childbtn3D.init(baseName);
        }
    }

    public virtual void setDefault()
    {
        valR = new Dictionary<string, string>();
        valU = new Dictionary<string, string>();
        initBaseChild();
    }

    public void setLocation(int num)
    {
        if (curLocation >= 0 && curLocation < locations.Count)
        {
            locations[curLocation].camDistanse = Main.Instance.orbitCamera.distance;
            locations[curLocation].cameraRot = Main.Instance.orbitCamera.transform.eulerAngles;
        }

        for (int i = 0; i < locations.Count; i++)
        {
            if (i == num)
            {
                curLocation = i;
                locations[i].objLocation.SetActive(true);
                Main.Instance.setOrbitCamera(locations[i].mainTarget);
                Main.Instance.orbitCamera.init (locations[i].camDistanse, locations[i].cameraPos, locations[i].cameraRot);
            }
            else
            {
                locations[i].objLocation.SetActive(false);
            }
        }
    }

    public virtual void onClose()
    {
        ModelController.destroyModel(baseName);
    }

    public float getR(string conn1, string conn2)
    {
        string curKey = conn1 + ":" + conn2;
        string curR = "";
        
        if (valR.ContainsKey(curKey)) curR = valR[curKey];
        curKey = conn2 + ":" + conn1;
        if (valR.ContainsKey(curKey)) curR = valR[curKey];

        if (conn1 == conn2)
            return -1;
        
        if (curR != "")
        {
            return Main.ModelController.getVarCalc(baseName, curR);
        }

        return -1;
    }

    public float getU(Contact conn1, Contact conn2)
    {
        if (conn1 == conn2)
            return 0;

        string curKey = conn1.var + ":" + conn2.var;
        string curU = "";
        
        int alterU = 1; // 1 - переменный, 0 - не связанн, -1 - постоянный
        
        if (conn1.var[0] == 'U' && conn2.var[0] == 'U') alterU = 1;
        else if (conn1.var[0] == 'u' && conn2.var[0] == 'u') alterU = -1;
        else if (conn1.var[0] == 'U' && conn2.var[0] == 'u') alterU = 0;
        else if (conn1.var[0] == 'u' && conn2.var[0] == 'U') alterU = 0;
        else if (conn1.var[0] == 'U' && conn2.var[0] == 'u') alterU = 0;
        else if (conn1.var[0] == 'U') alterU = 1;
        else if (conn2.var[0] == 'U') alterU = 1;
        else if (conn1.var[0] == 'u') alterU = -1;
        else if (conn2.var[0] == 'u') alterU = -1;

        float sign= 1;
        
        if (valU.ContainsKey(curKey)) curU = valU[curKey];
        curKey = conn2.var + ":" + conn1.var;
        if (valU.ContainsKey(curKey))
        {
            curU = valU[curKey];
            sign = -1;
        }

        if (alterU == 1) sign = 1; // переменный

        float kObriv = 1;
        if (alterU == 0) kObriv = 0;
        else if (alterU == 1 && conn1.U == obrivU) kObriv = 0;
        else if (alterU == 1 && conn2.U == obrivU) kObriv = 0;

        if (curU != "")
        {
            return sign * kObriv * Main.ModelController.getVarCalc(baseName, curU);
        }

        if (alterU == 1)
        {
            return sign * kObriv * 1.72f * (conn1.U + conn2.U) / 2f;
        }

        return sign * kObriv * (conn1.U - conn2.U);
    }
}

