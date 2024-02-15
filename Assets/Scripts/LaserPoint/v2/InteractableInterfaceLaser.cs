using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;

public class InteractableInterfaceLaser : MonoBehaviour
{
    [SerializeField]
    public SteamVR_LaserPointer laserPointer;
    public bool selected;

    private Outline backlight;


    //TODO: Add the ability to customize the selection and the ability to upload settings to the server
    private void OutlineConfig(Outline script)
    {
        script.OutlineColor = Color.yellow;
        script.OutlineMode = Outline.Mode.OutlineVisible;
        script.enabled = false;
    }


    private void Awake()
    {
        backlight = gameObject.AddComponent<Outline>();
        OutlineConfig(backlight);
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        selected = false;
    }

    void Start()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        selected = false;
    }
    public void PointerInside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == false)
        {
            selected = true;
            backlight.enabled = true;
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == true)
        {
            selected = false;
            backlight.enabled = false;
        }
    }

    public bool get_selected_value()
    {
        return selected;
    }
}