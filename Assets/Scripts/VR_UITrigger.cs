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
                inputInstance = collision.gameObject.GetComponentInParent<vThirdPersonInput>(); //Ссылка на игрового персонажа
            }

            foreach (var item in canvases)
            {
                item.enabled = true;
            }

            //Подписка на событие нажатия клавишы взаимодействия
            inputInstance.onActionButtonDown.AddListener(testTrigger);

            OnTriggerEvent.Invoke(); // Вызываем все функции, указанные в инспекторе.

            var list = GetComponents<IEnterTrigger>();// Реализация интерфейса
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
        Debug.Log("Отожмусь 20-ку если заработаешь!");
    }
}
