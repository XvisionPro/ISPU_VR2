using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_UITrigger : MonoBehaviour
{
    public static GameObject instance;
    private Canvas[] canvases;

    void Awake()
    {
        canvases = this.GetComponentsInChildren<Canvas>();

        // Initialization
        foreach (var item in canvases)
        {
            item.enabled = false;
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
       if(collision.gameObject.tag == "PlayerUI")
        {
            foreach (var item in canvases)
            {
                //item.GetComponent<CanvasGroup>().alpha = 0;
                item.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerUI")
        {
            foreach (var item in canvases)
            {
                item.enabled = false;
            }
        }
    }
}
