using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]
    private float distance = 0.2f;
    private MeshRenderer mesh;

    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
            /*.material.EnableKeyword("_EMISSION");
        x.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.yellow);*/
    }

    private void Update()
    {
        if (mesh != null)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0.5f);
                mesh.material.SetColor("_EmissionColor", Color.yellow);
                mesh.material.EnableKeyword("_EMISSION");
            }
            else
            {
                mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0);
                mesh.material.SetColor("_EmissionColor", Color.black);
                mesh.material.EnableKeyword("_EMISSION");
            }
        }
    }

    void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Main.Instance.setOrbitCamera(transform);
            Main.Instance.orbitCamera.distance = -distance;
        }
    }
}
