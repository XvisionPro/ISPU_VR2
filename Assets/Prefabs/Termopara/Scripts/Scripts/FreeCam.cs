using UnityEngine;

public class FreeCam : MonoBehaviour
{
    // Скорость движения камеры
    public float movementSpeed = 10.0f;
    // Чувствительность мыши при повороте камеры
    public float mouseSensitivity = 3.0f;

    private Vector3 movement = Vector3.zero;

    void Update()
    {
        // Движение по осям WASD
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Движение по оси Y (вертикальное)
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

        // Нормализуем вектор движения, чтобы скорость была одинаковой во всех направлениях
        movement.Normalize();

        // Вращение камеры по нажатию на колесико мыши
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.right, -mouseY, Space.Self);
        }

        // Движение камеры по осям WASD
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.Self);
    }
}
