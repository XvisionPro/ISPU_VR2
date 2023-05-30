using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateleft : MonoBehaviour
{
    public rotate rot;
    private Vector3 basepos;
    private Vector3 needpos;
    private bool work = false;
    // Start is called before the first frame update
    void Start()
    {
        basepos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        rot.Rotationleft();
    }
}
