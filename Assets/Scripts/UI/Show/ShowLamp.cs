using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowLamp : MonoBehaviour
{
    public string par;
    public Image onState;
    public Image ofState;

    [SerializeField]
    private float onVal = 1;
    [SerializeField]
    private float offVal = 0;

    private string baseName = "";
    private float val = 0;

    public void init(string _baseName)
    {
        baseName = _baseName;
    }

    void Update()
    {
        val = BaseUtils.toFloat(Main.ModelController.getVar(baseName + par));

        if (val == onVal) onState.gameObject.SetActive(true);
        else onState.gameObject.SetActive(false);

        if (val == offVal) ofState.gameObject.SetActive(true);
        else ofState.gameObject.SetActive(false);
    }
}