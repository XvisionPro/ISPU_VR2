using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VButtonUI : MonoBehaviour
{
    public bool clickVButton = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        clickVButton = true;
    }
}
