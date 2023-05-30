using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCam : MonoBehaviour
{

    public float speed = 10.0f;

    public float horizontalSpeed = 2.0f; // ���������������� ���� �� �����������
    public float verticalSpeed = 2.0f; // ���������������� ���� �� ���������
    public float minYAngle = -90.0f; // ����������� ���� �� ��� Y
    public float maxYAngle = 90.0f; // ������������ ���� �� ��� Y
    private float rotationX = -180.0f; // ���� �������� �� ��� X
    private float rotationY = 0.0f; // ���� �������� �� ��� Y

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
            rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

            // ������� ������ ������ ���� X � Y
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        float horizontalwalk = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(horizontalwalk, 0, vertical);
    }
}

