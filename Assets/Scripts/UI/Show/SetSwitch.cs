using Assets.Scripts.Logic;
using UnityEngine;
using UnityEngine.UI;

public class SetSwitch : MonoBehaviour
{
    public BaseModel baseModel;
    public Animator anim;
    public string par;
    public float val;
    public float onVal = 1;
    public float offVal = 0;

    void Start()
    {

    }

    public void OnMouseDown()
    {
        if (Main.Instance != null)
        {
            if (val == onVal)
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseModel.baseName + par + "=" + offVal);
            else
                Main.Instance.network.send(AllTypes.MESS_DATA + ":" + baseModel.baseName + par + "=" + onVal);
        }
    }

    void Update()
    {
        if (Main.Instance != null)
        {
            val = BaseUtils.toFloat(Main.ModelController.getVar(baseModel.baseName + par));
            if (anim != null) anim.SetFloat("val", val);
        }
    }
}