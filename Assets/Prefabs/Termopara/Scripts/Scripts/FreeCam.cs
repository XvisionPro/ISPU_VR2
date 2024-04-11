using UnityEngine;

public class FreeCam : MonoBehaviour
{
    // �������� �������� ������
    public float movementSpeed = 10.0f;
    // ���������������� ���� ��� �������� ������
    public float mouseSensitivity = 3.0f;

    private Vector3 movement = Vector3.zero;

    void Update()
    {
        // �������� �� ���� WASD
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // �������� �� ��� Y (������������)
        if (Input.GetKey(KeyCode.Q))
        {
            movement.y = -1.0f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            movement.y = 1.0f;
        }
        else
        {
            movement.y = 0.0f;
        }

        // ����������� ������ ��������, ����� �������� ���� ���������� �� ���� ������������
        movement.Normalize();

        // �������� ������ �� ������� �� �������� ����
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.right, -mouseY, Space.Self);
        }

        // �������� ������ �� ���� WASD
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.Self);
    }
}
