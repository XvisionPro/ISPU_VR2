using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator anim;
    public float jumpForce = 3.5f;
    public float walkingSpeed = 2f;
    public float runningSpeed = 6f;
    public float currentSpeed;
    private float animationInterpolation = 1f;
    [SerializeField]
    public Camera Kamera1;
    public float Health=50;
    Vector3 velocity;
    public float speed = 10f;
    public float gravity = -9.8f;
    public float jumpHeight = 50f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;//Маска самой земли
    private CharacterController CharacterController; //Ссылка на объект, который висит на velocity
    private Camera camera;
    private Vector3 needpos;
    private Vector3 needpos3d;
    private Vector3 basepos;
    bool isGrounded; //Проверка на землю
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Отключаем наш указатель при игре, ШОБ НЕ МЕШАЛСЯ
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>(); // Получаем, собственно, сам объект
        if (CharacterController == null) //Проверка его наличия (объекта)
        {
            Debug.Log("Charactercontroller is NULLLLLL");
        }
        if (anim == null)
        {
            anim = GameObject.Find("Ch17_nonPBR").GetComponent<Animator>();
        }
        if (camera == null)
        {
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
    private void OnGUI() // Создание прицела
    {
        int size = 12;
        float PosX = Kamera1.pixelWidth / 2 - size / 4;
        float PosY = Kamera1.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(PosX, PosY, size, size), "[]");
    }
    void Run()
    {
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
        anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
    }
    void Walk()
    {
        // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
        // Time.deltaTime - это время между этим кадром и предыдущим кадром. Это позволяет плавно переходить с одного числа до второго НЕЗАВИСИМО ОТ КАДРОВ В СЕКУНДУ (FPS)!!!
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
        anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Kamera1.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Проверяем, находится ли блядский объект на земле
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if(Input.GetKey(KeyCode.Alpha3)) 
        {
            needpos3d = new Vector3(0f, 2.24f, -3.5f);
            Kamera1.transform.localPosition = Vector3.Lerp(a: basepos, b: needpos3d, t: 5f);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            needpos3d = new Vector3(0f, 1.24f, 0.22f);
            Kamera1.transform.localPosition = Vector3.Lerp(a: basepos, b: needpos3d, t: 5f);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            // Зажаты ли еще кнопки A S D?
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                // Если да, то мы идем пешком
                Walk();
            }
            // Если нет, то тогда бежим!
            else
            {
                Run();
            }
        }
        // Если W & Shift не зажаты, то мы просто идем пешком
        else
        {
            Walk();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5;
        }
        else
            speed = 2;

        //ПРЫЖКИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИИ
        ////Если зажат пробел, то в аниматоре отправляем сообщение тригеру, который активирует анимацию прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * deltaX + transform.forward * deltaZ; //Записываем в вектор движения нажатия наших кномпочек
        CharacterController.Move(move * speed * Time.deltaTime); //Задаём скорость персонажу
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;//Задаём гравитейшион персонажу
        CharacterController.Move(velocity * Time.deltaTime);
    }
    //transform.Translate(deltaX, 0, deltaZ); //применяем движение к персонажу
}
