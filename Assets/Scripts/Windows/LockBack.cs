using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockBack : MonoBehaviour, IPointerClickHandler
{
    public Image image;
    public Action callback;

    void Start ()
    {
        image.alphaHitTestMinimumThreshold = 0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (callback!=null) callback();
    }

    void OnMouseDown()
    {
        
    }
}
