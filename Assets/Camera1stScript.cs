using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Camera1stScript : MonoBehaviour
{
    private bool working=true;
    [SerializeField]
    private GameObject TextE;
    [SerializeField]
    private TextMeshProUGUI TextCanvas;
    [SerializeField]
    private Transform thing;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private GameObject PlayerCamera;
    [SerializeField]
    private GameObject CameraMultimetr;
    [SerializeField]
    private int attackDistance = 3;
    public float speed = 10.0f;

    public float horizontalSpeed = 2.0f; // ���������������������������������
    public float verticalSpeed = 2.0f; // ���������������� ���� �� ���������
    public float minYAngle = -90.0f; // ����������� ���� �� ��� Y
    public float maxYAngle = 90.0f; // ������������ ���� �� ��� Y
    private float rotationX = -180.0f; // ���� �������� �� ��� X
    private float rotationY = 0.0f; // ���� �������� �� ��� Y


    // Start is called before the first frame update
    void Start()
    {
        TextCanvas = (TextMeshProUGUI)GameObject.Find("TextMult").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) <= attackDistance)
        {
            TextE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && working)
            {
                Debug.Log("� �������");
                PlayerCamera.SetActive(false);
                CameraMultimetr.SetActive(true);
                TextCanvas.text = "��� - ��������������\n��������� ��������\n��� - �������� ������";
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                working = false;
            }
            else if (Input.GetKeyDown(KeyCode.E) && working == false)
            {
                Debug.Log("� �������");
                PlayerCamera.SetActive(true);
                CameraMultimetr.SetActive(false);
                TextCanvas.text = "";
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                working = true;
            }
        }
        else
        {
            TextE.SetActive(false);
        }
    }
}
