using Invector.vCamera;
using Invector.vCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VR_UITrigger : MonoBehaviour
{
    public static vThirdPersonInput inputInstance;
    public static vThirdPersonCamera cameraInstance;
    public Camera[] cameras;
    public UnityEvent OnInteractEvent;

    #region variables
    private Canvas[] canvases;
    private GenericInput cameraSwitch = new GenericInput("Switch", "", "");
    private int _currentCamera;
    #endregion
    public int currentCamera 
    {
        get
        {
            return _currentCamera;
        }
        set {
            if(_currentCamera + value >= cameras.Length)
            {
                _currentCamera = 0;
            }
            else if(_currentCamera + value < 0)
            {
                _currentCamera = cameras.Length - 1;
            }
            else { 
                _currentCamera = value;
            }
        }
    }

    void Awake()
    {
        canvases = this.GetComponentsInChildren<Canvas>();
        // Initialization
        foreach (var item in canvases)
        {
            item.enabled = false;
        }
        foreach (var item in cameras)
        {
            item.enabled = false;
        }
    }

    private void Update()
    {
        if (inputInstance == null || Time.timeScale == 0)
        {
            return;
        }

        inputHandler();
    }

    private void inputHandler()
    {
        if (inputInstance.isAction && cameraSwitch.GetButtonDown())
        {
            Debug.Log(cameraSwitch.GetAxis());
            if (cameraSwitch.GetAxis() > 0) //TODO: ������� ���, ���� � ��� ������� ���� ����������� ����� ������
            {
                changeCamera(false);
            }
            else
            {
                changeCamera(true);
            }
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
        }
    }

    private void changeCamera(bool isReverse)
    {
        cameras[currentCamera].enabled = !cameras[currentCamera].enabled;
        currentCamera += isReverse ? -1 : 1; //TODO: ���������� �� Axis 
        cameras[currentCamera].enabled = !cameras[currentCamera].enabled;
    }

    public void FreezeMovement()
    {
        if (inputInstance.isAction)
        {
            cameraInstance.FreezeCamera();
            inputInstance.LockCursor(true);
            inputInstance.ShowCursor(true);
            inputInstance.SetLockCameraInput(true);
            inputInstance.lockMoveInput = true;
        }
        else
        {
            cameraInstance.UnFreezeCamera();
            inputInstance.LockCursor(false);
            inputInstance.ShowCursor(false);
            inputInstance.SetLockCameraInput(false);
            inputInstance.lockMoveInput = false;
        }
    }

    public void SetCameraToObject()
    {
        if(inputInstance.isAction && cameras.Length > 0)
        {
            cameraInstance.targetCamera.enabled = !cameraInstance.targetCamera.enabled;
            cameras[0].enabled = !cameras[0].enabled; //������ ����� ������.
            currentCamera = 0;

        }
        else
        {
            cameraInstance.targetCamera.enabled = true;
            foreach (var item in cameras)
            {
                item.enabled = false;
            }
        }
    }
}
