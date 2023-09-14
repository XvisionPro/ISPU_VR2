using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMesureLine : MonoBehaviour
{
    public Contact cont;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPosition(Contact _cont)
    {
        if (cont == _cont)
        {
            Clear();
        }
        else
        {
            cont = _cont;
            transform.SetParent(Main.Instance.Scene);
            //transform.SetParent(cont.transform);
            transform.position = cont.transform.position;
            gameObject.SetActive(true);
            
            Vector3 newDirection = Camera.main.transform.position - cont.transform.position;
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        Main.ModelController.showMesure(true);
    }

    public void Clear()
    {
        cont = null;
        transform.SetParent(Main.Instance.Scene);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
