using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Graduirovka : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.UI.Button HK;
    public UnityEngine.UI.Button HA;
    public UnityEngine.UI.Button PP;
    public float alpha;

    void Start()
    {
        HK.onClick.AddListener(OnClickHK);
        HA.onClick.AddListener(OnClickHA);
        PP.onClick.AddListener(OnClickPP);
    }

    void OnClickHK()
    {
        alpha = 40f;
    }

    void OnClickHA()
    {
        alpha = 41f;
    }

    void OnClickPP()
    {
        alpha = 7.9f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
