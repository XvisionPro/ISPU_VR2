using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class ButtonScript : MonoBehaviour
{
    public static SteamVR_LaserPointer laserPointer;

    public bool selected;
    public static Hand hand;
    // Start is called before the first frame update
    void Start()
    {
        selected = false;
    }

    public void Activete()
    {
        //laserPointer = (Laser)GameObject.Find("RightHand").GetComponent<Laser>();
        //hand = (Hand)GameObject.Find("RightHand").GetComponent<Hand>();
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.CompareTag("Interact"))
        {
            gameObject.GetComponent<Outline>().enabled = true;
            selected = true;
            Debug.Log("pointer is inside this object" + e.target.name);
        }
/*        if (e.target.name == this.gameObject.name && selected == false)
        
            gameObject.GetComponent<Outline>().enabled = true;*/
        //    selected = true;
        //    Debug.Log("pointer is inside this object" + e.target.name);
        //}
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {

        if (e.target.name == this.gameObject.name && selected == true)
        {
            gameObject.GetComponent<Outline>().enabled = false;
            selected = false;
            Debug.Log("pointer is outside this object" + e.target.name);
        }
    }

    public void PointerClick<T>(object sender, PointerEventArgs e) where T : EventTrigger
    {
        if (e.target.name == this.gameObject.name)
        {
            //gameObject.SetActive(false);
            //Debug.Log("ClickClickClickClickClickClickClickClickClickClick");
            var obj =(GameObject)gameObject.GetComponent<T>().OnMouseDown();
        }
    }

    public bool get_selected_value()
    {
        return selected;
    }
}