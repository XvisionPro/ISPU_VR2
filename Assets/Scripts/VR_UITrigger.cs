using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VR_UITrigger : MonoBehaviour
{
    public static vThirdPersonInput inputInstance;
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
            if(inputInstance == null)
            {
                inputInstance = collision.gameObject.GetComponentInParent<vThirdPersonInput>(); //������ �� �������� ���������
            }

            foreach (var item in canvases)
            {
                item.enabled = true;
            }

            //�������� �� ������� ������� ������� ��������������
            inputInstance.onActionButtonDown.AddListener(testTrigger);

            OnTriggerEvent.Invoke(); // �������� ��� �������, ��������� � ����������.

            var list = GetComponents<IEnterTrigger>();// ���������� ����������
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
            inputInstance.onActionButtonDown.RemoveAllListeners();

            Debug.Log("Out");
        }
    }

    public void testTrigger()
    {
        Debug.Log("�������� 20-�� ���� �����������!");
    }
}
