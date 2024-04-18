using Invector.vCamera;
using Invector.vCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VR_UITrigger : MonoBehaviour
{
    public static vThirdPersonInput inputInstance;
    public static vThirdPersonCamera cameraInstance;
    [SerializeField]
    private Camera[] cameras;
    public UnityEvent OnInteractEvent;

    #region variables
    private Canvas[] canvases;
    private GenericInput cameraSwitch = new GenericInput("Switch", "", "");
    private int _currentCamera;
    private bool isAction = false;
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
        //TODO: ������� �������� �� ������� ����� � cameras. ���� ����� ���, �� �������� ������ �� ������������
        foreach (var item in cameras)
        {
            PhysicsRaycaster physicsRaycaster = item.GetComponent<PhysicsRaycaster>();
            if (physicsRaycaster == null)
            {
                Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
            }
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
        if (inputInstance.isAction && cameraSwitch.GetButtonDown() && isAction)
        {
            Debug.Log(cameraSwitch.GetAxis());
            if (cameraSwitch.GetAxis() > 0) //TODO: ������� ���, ���� � ��� ������� ���� ����������� ����� ������
            {
                changeCamera(false);
                Debug.Log(cameras.ToString());
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

            //TODO: ������� UI �����������
            inputInstance.hud.ShowInteract(inputInstance.actionInput);
            isAction = true; //� � ����������
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
            inputInstance.hud.HideInteract();
            isAction = false;// � �� � ����������
        }
    }

    private void changeCamera(bool isReverse)
    {
        if(cameras.Length > 1)
        {
            cameras[currentCamera].enabled = !cameras[currentCamera].enabled;
            currentCamera += isReverse ? -1 : 1; //TODO: ���������� �� Axis 
            cameras[currentCamera].enabled = !cameras[currentCamera].enabled;
        }
        else
        {

        }
    }

    public void FreezeMovement()
    {
        // TODO:  ���� ��������� ������
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
