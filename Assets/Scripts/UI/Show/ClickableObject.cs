using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    private Action onLeftClick;
    private Action onRightClick;
    private Action onMiddleClick;

    public void init(Action _onLeftClick, Action _onRightClick, Action _onMiddleClick)
    {
        onLeftClick = _onLeftClick;
        onRightClick = _onRightClick;
        onMiddleClick = _onMiddleClick;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (onLeftClick != null) onLeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (onMiddleClick != null) onMiddleClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (onRightClick != null) onRightClick();
        }
    }
}