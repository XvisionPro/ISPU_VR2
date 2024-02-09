using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class xyu : Button
{
    public static SteamVR_LaserPointer laserPointer;


    [SerializeField]
    public CustomEvents.UnityEventHand curClick;
    [SerializeField]
    public Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        if (outline == null) outline = gameObject.GetComponent<Outline>();
        onClick.RemoveAllListeners();
        onClick.AddListener(Click);
    }

    public void Activete()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("Interact"))
        {
            if (outline != null) outline.enabled = true;
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name)
        {
            if (outline != null) outline.enabled = false;
        }
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == this.gameObject.name)
        {
            Click();
        }
    }

    private void OnMouseOver()
    {
        if (outline != null) outline.enabled = false;
    }

    private void OnMouseEnter()
    {
        if (outline != null) outline.enabled = true;
    }

    public void Click()
    {
        if (outline != null) outline.enabled = true;

        if (onClick != null) curClick.Invoke(null);
    }
}
