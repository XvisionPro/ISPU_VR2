using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Valve.VR.Extras;


public class Laser : SteamVR_LaserPointer
{

    public override void OnPointerIn(PointerEventArgs e)
    {
        base.OnPointerIn(e);
        e.target.GetComponent<IPointerEnterHandler>()?.OnPointerEnter(new PointerEventData(EventSystem.current));

    }

    public override void OnPointerClick(PointerEventArgs e)
    {
        base.OnPointerClick(e);
        e.target.GetComponent<IPointerClickHandler>()?.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    public override void OnPointerOut(PointerEventArgs e)
    {
        base.OnPointerOut(e);
        e.target.GetComponent<IPointerExitHandler>()?.OnPointerExit(new PointerEventData(EventSystem.current));

    }
}
