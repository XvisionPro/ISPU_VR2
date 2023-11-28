using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using Valve.VR.Extras;


public class ButtonScript : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;

    public bool selected;
    // Start is called before the first frame update
    void Start()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == false)
        {
            gameObject.GetComponent<Outline>().enabled = false;
            selected = true;
            Debug.Log("pointer is inside this object" + e.target.name);
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == true)
        {
            gameObject.GetComponent<Outline>().enabled = true;
            selected = false;
            Debug.Log("pointer is outside this object" + e.target.name);
        }
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            gameObject.SetActive(false);
            Debug.Log("ClickClickClickClickClickClickClickClickClickClick");
        }
    }

    public bool get_selected_value()
    {
        return selected;
    }
}