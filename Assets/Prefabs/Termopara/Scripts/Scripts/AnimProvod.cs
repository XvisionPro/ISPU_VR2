using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimProvod : MonoBehaviour
{
    private Animator anim;
    public bool provodIsOpening = false;
    public bool provodIsClosing = false;
    public bool isOpen = false;
    public MoveDetals shaiba2;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (shaiba2.flagIsOpen && !provodIsOpening && !isOpen)
            {
                provodIsOpening = true;
            }
            if (shaiba2.flagIsOpen && isOpen)
            {
                provodIsClosing = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); // получаем компонент Animator 
    }

    // Update is called once per frame
    void Update()
    {
        if (provodIsOpening && gameObject.tag == "provodleft")
        {
            anim.Play("ProvodStartGib");
            provodIsOpening = false;
            isOpen = true;
        }
        if (provodIsClosing && gameObject.tag == "provodleft")
        {
            anim.Play("CloseProvodLeft");
            provodIsClosing= false;
            isOpen= false;
        }

        if (provodIsOpening && gameObject.tag == "provodright")
        {
            anim.Play("ProvodStartGib2");
            provodIsOpening = false;
            isOpen = true;
        }
        if (provodIsClosing && gameObject.tag == "provodright")
        {
            anim.Play("CloseProvodRight");
            provodIsClosing = false;
            isOpen = false;
        }
    }
}
