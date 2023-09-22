using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(OpenVR.IsHmdPresent().ToString());
    }


	// Update is called once per frame
	void Update()
        {
        
        }

    }
