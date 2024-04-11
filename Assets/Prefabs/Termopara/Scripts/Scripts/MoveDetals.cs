using System.Collections;
using UnityEngine;

public class MoveDetals : MonoBehaviour
{
    public float rotationSpeed = 500f;
    public float translationSpeed = 0.5f;
    private float distanceTraveled = 0f;
    public float stoppingDistance;
    public MoveDetals parent;
    public MoveDetals child;
    public bool isRotateOpen = false;
    public bool isRotateClose = false;
    public bool flagIsOpen = false;
    public AnimProvod provod;

    private void OnMouseDown()
    {
        if ((parent == null || parent.flagIsOpen) && !flagIsOpen && !isRotateClose && provod == null)
        {
            isRotateOpen = true;
        }
        
        if ((child == null || !child.flagIsOpen) && flagIsOpen && !isRotateOpen && provod == null)
        {
            isRotateClose = true;
        }
        if (provod != null)
        {
            if ((parent == null || parent.flagIsOpen) && !flagIsOpen && !isRotateClose && !provod.isOpen)
            {
                isRotateOpen = true;
            }
            if ((child == null || !child.flagIsOpen) && flagIsOpen && !isRotateOpen && !provod.isOpen)
            {
                isRotateClose = true;
            }
        }
    }

    // Вызывается при откручивании гайки
    private void OnUnscrew()
    {
        Vector3 direction = new Vector3(1, 1, 0);
        transform.Translate(direction * translationSpeed * Time.deltaTime); // перемещение
        transform.RotateAround(transform.position, direction, rotationSpeed * Time.deltaTime); // вращение
        distanceTraveled += translationSpeed * Time.deltaTime;
    }

    private void OnScrew()
    {

        Vector3 direction = new Vector3(1, 1, 0);
        transform.Translate(-direction * translationSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, -direction, rotationSpeed * Time.deltaTime);
        distanceTraveled -= translationSpeed * Time.deltaTime;
    }

    private void Update()
    {
        if (distanceTraveled >= stoppingDistance)
        {
            isRotateOpen = false;
        }

        if (distanceTraveled < 0)
        {
            isRotateClose = false;
        }
        if (isRotateOpen)
        {
            OnUnscrew();
            flagIsOpen = true;
        }
        if (isRotateClose)
        {
            OnScrew();
            flagIsOpen = false;
        }
    }
}
