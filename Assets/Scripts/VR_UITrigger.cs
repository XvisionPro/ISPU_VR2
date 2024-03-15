using Invector.vCamera;
using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VR_UITrigger : MonoBehaviour
{
    public static vThirdPersonInput inputInstance;
    public static vThirdPersonCamera cameraInstance;
    private Canvas[] canvases;
    public UnityEvent OnInteractEvent;

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
            if(inputInstance == null)
            {
                inputInstance = collision.gameObject.GetComponentInParent<vThirdPersonInput>(); //������ �� �������� ���������
                cameraInstance = inputInstance.tpCamera; //������ �� ������ ���������
            }

            foreach (var item in canvases)
            {
                item.enabled = true;
            }

            //�������� �� ������� ������� ������� ��������������
            inputInstance.onActionButtonDown = OnInteractEvent;

            var list = GetComponents<IEnterTrigger>();// ���������� ���������� ��� ������ � �������
            foreach (var item in list)
            {
                item.EnterTrigger(inputInstance);
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
            inputInstance.onActionButtonDown = new UnityEvent();
            Debug.Log("Out");
        }
    }

    public void SetCameraToObject()
    {
        cameraInstance.FreezeCamera();
        inputInstance.LockCursor(true);
        inputInstance.ShowCursor(true);
    }
}
