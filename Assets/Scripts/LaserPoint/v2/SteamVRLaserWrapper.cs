using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;

public class SteamVRLaserWrapper : MonoBehaviour
{
    private SteamVR_LaserPointer _steamVrLaserPointer;

    private void Awake()
    {
        _steamVrLaserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
        _steamVrLaserPointer.PointerIn += OnPointerIn;
        _steamVrLaserPointer.PointerOut += OnPointerOut;
        _steamVrLaserPointer.PointerClick += OnPointerClick;
    }

    private void OnPointerClick(object sender, PointerEventArgs e)
    {
        IPointerClickHandler clickHandler = e.target.GetComponent<IPointerClickHandler>();
        if (clickHandler == null)
        {
            return;
        }

        Debug.Log(sender);
        clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        IPointerExitHandler pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();
        if (pointerExitHandler == null)
        {
            return;
        }

        pointerExitHandler.OnPointerExit(new PointerEventData(EventSystem.current));
    }

    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        IPointerEnterHandler pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();
        if (pointerEnterHandler == null)
        {
            return;
        }

        pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));
    }
}