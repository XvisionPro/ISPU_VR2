using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour
{
    public Hashtable base_obj = new Hashtable();

    public void Initializ_base_link(GameObject[] arr_base_obj)
    {
        foreach (GameObject obj in arr_base_obj)
        {
            base_obj[obj.name] = obj;
        }
    }
}
