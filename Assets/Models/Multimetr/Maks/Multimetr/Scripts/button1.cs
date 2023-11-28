using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class button1 : MonoBehaviour
{
    private Action onChange;
    private Vector3 basepos;
    private Vector3 needpos;
    public float distance = -0.3f;
    public bool work1 = false;

    public SteamVR_LaserPointer laserPointer;
    public bool selected;

    public void PointerInside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == false)
        {
            gameObject.GetComponent<Outline>().enabled = true;
            selected = true;
        }
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name && selected == true)
        {
            gameObject.GetComponent<Outline>().enabled = false;
            selected = false;
        }
    }
    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            OnMouseDown();
        }
    }
    public bool get_selected_value()
    {
        return selected;
    }


    // Start is called before the first frame update
    void Start()
    {
        basepos = gameObject.transform.localPosition;
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
        selected = false;
    }

    public void OnMouseDown()
    {
        if (work1 == false)
        {
            needpos = new Vector3(gameObject.transform.localPosition.x + distance, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
            gameObject.transform.localPosition = needpos;
            work1 = true;
        }
        else if (work1 == true)
        {
            gameObject.transform.localPosition = basepos;
            work1 = false;
        }
        if (onChange != null)
        {
            onChange();
        }
    }
    public void Button1Click(Action on_change)
    {
        onChange = on_change;
    }
}
