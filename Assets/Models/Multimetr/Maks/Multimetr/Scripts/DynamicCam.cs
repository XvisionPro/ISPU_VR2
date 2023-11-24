using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCam : MonoBehaviour
{

    public float speed = 10.0f;

    public float horizontalSpeed = 2.0f; // ���������������� ���� �� �����������
    public float verticalSpeed = 2.0f; // ���������������� ���� �� ���������
    public float minYAngleY = -90.0f; // ����������� ���� �� ��� Y
    public float maxYAngleY = 90.0f; // ������������ ���� �� ��� Y
    private float rotationX = -180.0f; // ���� �������� �� ��� X
    private float rotationY = -82.59f; // ���� �������� �� ��� Y
    public float minYAngleX = -90.0f; // ����������� ���� �� ��� Y
    public float maxYAngleX = 90.0f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // �������� ������ ���� � ��������� ��� � ������ ������
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // ���������, ������ �� ����� ������ ����
        {
            // �������� ������� �������� ��� ����������� ���� � �������� ���� �������� �� ��� X
            rotationX += Input.GetAxis("Mouse X") * horizontalSpeed;
            // �������� ������� �������� ��� ��������� ���� � �������� ���� �������� �� ��� Y
            rotationY += Input.GetAxis("Mouse Y") * verticalSpeed;

            // ������������ ���� �������� �� ��� Y ����� ����������� � ������������ ����������
            rotationY = Mathf.Clamp(rotationY, minYAngleY, maxYAngleY);
            rotationX = Mathf.Clamp(rotationX, minYAngleX, maxYAngleX);

            // ������� ������ ������ ���� X � Y
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }
}

