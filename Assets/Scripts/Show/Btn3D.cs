using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Btn3D: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    /*public static Action<string> onBtnDown;

    public class TestStringEvent : UnityEvent<string>
    {
    }*/

    public Highlight highlight;
    private bool btnON = false;
    private Vector3 needPos;
    public float speedPos  = 0.5f;
    public Vector3 deltaOn;
    private Vector3 basePos;
    private Action<bool> Callback;

    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.localPosition;
        needPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (needPos != transform.localPosition)
        {
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, needPos, speedPos);
        }
    }

    public void init(Action<bool> _callback)
    {
        Callback = _callback;
    }

    //private void OnMouseOver()
    //{
    //    if (highlight != null) highlight.ToggleHighlight(false);
    //}

    //private void OnMouseEnter()
    //{
    //    if (highlight != null) highlight.ToggleHighlight(true);
    //}

    public void Click()
    {
        if (highlight != null) highlight.ToggleHighlight(true);
        btnON = !btnON;
        if (Callback != null) Callback(btnON);

        if (btnON)
        {
            needPos = basePos + deltaOn;
        }
        else
        {
            needPos = basePos;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlight != null) highlight.ToggleHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlight != null) highlight.ToggleHighlight(false);
    }
}
