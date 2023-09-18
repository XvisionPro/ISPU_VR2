using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePivotPoint : MonoBehaviour
{
    public GameObject Player, Pivot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Pivot.transform.localPosition = new Vector3(Player.transform.localPosition.x, 0, Player.transform.localPosition.z);
    }
}
