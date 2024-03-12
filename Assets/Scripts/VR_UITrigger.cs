using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VR_UITrigger : MonoBehaviour
{
    public static GameObject instance;
    private Canvas[] canvases;
    public UnityEvent OnTriggerEvent;

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
                item.enabled = true;
            }
            var list = GetComponents<IEnterTrigger>();
            foreach (var item in list)
            {
                item.EnterTrigger();
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
            Debug.Log("Out");
        }
    }

    
}
