using Assets.Scripts.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class btnToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Button btn;
    [SerializeField]
    private Button btnOf;
    [SerializeField]
    private Button btnOn;

    [SerializeField]
    private bool threeBtn = false;
    [SerializeField]
    private bool threePos= false;
    [SerializeField]
    private bool useMiddleBtnMouse = false;
    [SerializeField]
    private string field;

    [SerializeField]
    private Color onColor = Color.red;
    [SerializeField]
    private Color offColor = Color.white;

    [SerializeField]
    private bool hideObj = true;
    [SerializeField]
    private GameObject objBase;
    [SerializeField]
    private GameObject objOf;
    [SerializeField]
    private GameObject objOn;

    [SerializeField]
    private float baseVal = 0;
    [SerializeField]
    private float onVal = 1;
    [SerializeField]
    private float offVal = 0;

    private string baseName = "";
    private float val = 0;
    private float oldval = 9999;

    void Start()
    {
        
    }

    public void init(string _baseName)
    {
        baseName = _baseName;
        btn?.onClick.AddListener(onClick);
        btnOn?.onClick.AddListener(onOnClick);
        btnOf?.onClick.AddListener(onOfClick);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (threePos)
        {
            if (useMiddleBtnMouse)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    onOfClick();
                }
                else if (eventData.button == PointerEventData.InputButton.Middle)
                {
                    onClick();
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    onOnClick();
                }
            }
            else
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    onChangeClick(-1);
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    onChangeClick(1);
                }

            }
        }
    }

    void onChangeClick(float delta)
    {
        val = BaseUtils.toFloat(Main.ModelController.getVar(baseName + field));

        if (delta > 0)
        {
            if (val == offVal)
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + baseVal);
            else
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + onVal);
        }
        if (delta < 0)
        {
            if (val == onVal)
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + baseVal);
            else
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + offVal);
        }
    }

    void onClick()
    {
        val = BaseUtils.toFloat(Main.ModelController.getVar(baseName + field));
        
        if (threeBtn || threePos)
        {
            Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + baseVal);
        }
        else
        {
            if (val == onVal)
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + offVal);
            else
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + onVal);
        }
    }

    void onOfClick()
    {
        Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + offVal);
    }

    void onOnClick()
    {
        Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseName + field + "=" + onVal);
    }

    void Update()
    {
        if (baseName == "") return;

        val = BaseUtils.toFloat(Main.ModelController.getVar(baseName + field));
        if (oldval != val)
        {
            oldval = val;
            if (hideObj)
            {
                if (objOn != null) objOn.SetActive(false);
                if (objBase != null) objBase.SetActive(false);
                if (objOf != null) objOf.SetActive(false);
            }

            if (threeBtn || threePos)
            {
                if (val == baseVal)
                {
                    if (btn != null) btn.image.color = onColor;
                    if (objBase != null) objBase.SetActive(true);
                }
                else if (btn != null)
                {
                    btn.image.color = offColor;
                }

                if (val == offVal)
                {
                    if (btnOf != null) btnOf.image.color = onColor;
                    if (objOf != null) objOf.SetActive(true);

                }
                else if(btnOf != null) btnOf.image.color = offColor;

                if (val == onVal)
                {
                    if (btnOn != null) btnOn.image.color = onColor;
                    if (objOn != null) objOn.SetActive(true);
                }
                else if (btnOn != null) btnOn.image.color = offColor;
            }
            else
            {
                if (val > 0)
                {
                    btn.image.color = onColor;
                    if (objOn != null) objOn.SetActive(true);
                }
                else
                {
                    btn.image.color = offColor;
                    if (objBase != null) objBase.SetActive(true);
                }
            }
        }
    }
}
