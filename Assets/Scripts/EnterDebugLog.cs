using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDebugLog : MonoBehaviour, IEnterTrigger
{
    public void EnterTrigger(MonoBehaviour obj)
    {
        Debug.Log(obj.name);
    }

}
