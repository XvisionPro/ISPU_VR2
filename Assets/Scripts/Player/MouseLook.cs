using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private void Start()
    {
        Rigidbody body = GetComponent<Rigidbody>(); //—покойное вращение даже в текстурках
        if (body!=null)
        {
            body.freezeRotation = true;
        }
    }
    public Transform player;
    public enum RotationPlayer //переменные перемещени€ персонажа
    {
        XandY,
        X,
        Y
    }
    public RotationPlayer axes = RotationPlayer.XandY; //переменна€ этого класса, доступна в ёнити
    public float RotatoinSpeedHorizont = 5.0f; //скорость вращени€ персонажа мышкой
    public float RotatoinSpeedVertical = 5.0f;
    public float maxVertical = 45.0f;
    public float minVetrical = -45.0f;
    private float rotationX = 0;
    private void Update()
    {
        //ѕроверка оси движени€ персонажа
        if (axes==RotationPlayer.XandY)
        {
            rotationX -= Input.GetAxis("Mouse Y") * RotatoinSpeedVertical;
            rotationX = Mathf.Clamp(rotationX, minVetrical, maxVertical);

            float delta = Input.GetAxis("Mouse X")*RotatoinSpeedHorizont;
            float rotationY = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);//«аписываем в персонажа
        }
        else if (axes==RotationPlayer.X)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X")* RotatoinSpeedHorizont,0);
        }
        else if (axes==RotationPlayer.Y)
        {
            rotationX -= Input.GetAxis("Mouse Y") * RotatoinSpeedVertical;
            rotationX = Mathf.Clamp(rotationX, minVetrical, maxVertical);

            float rotationY = transform.localEulerAngles.y; //—охранение угла поворота по оси Y
            transform.localEulerAngles = new Vector3(rotationX,rotationY, 0);
        }
    }
}


