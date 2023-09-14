using TMPro;
using UnityEngine;

public class ShowVar : MonoBehaviour
{
    public string par;
    public TMP_Text valText;
    private string baseName = "";

    public void init(string _baseName)
    {
        baseName = _baseName;
    }

    void Update()
    {
        if (Main.Instance != null)
            if (valText != null) valText.text = Main.ModelController.getVar(baseName + par);
    }
}