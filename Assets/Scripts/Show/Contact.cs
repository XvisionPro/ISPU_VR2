using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour
{
    public BaseModel baseModel;
    public string var;
    public float U = 0;

    void Update()
    {
        if (var != "") U = BaseUtils.toFloat(Main.ModelController.getVar(baseModel.baseName + var));
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Main.ModelController.mesure1.setPosition(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Main.ModelController.mesure2.setPosition(this);
        }
    }
}
