using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    WithoutWeapon,
    PickingUpPM,
    WithPM
}

[RequireComponent (typeof (Rigidbody))]
public class CharacterController : MonoBehaviour {

    [SerializeField]Camera mainCamera;
	public float mouseSensitivity = 5f;                 //чувствительность мыши
    public float speed = 5f;                            //скорость персонажа
    public float runMultiple = 2f;                      //множитель бега
    public float maxVelocityChange = 10f;               //максимальное изменение скорости
    public float maxVerticalAngle = 60f;                //максимальное отклонение камеры по горизантали
    public float sitHeight = 0.75f;                     //высота присеста
    public PlayerState currentState;
    public GameObject hands;

    private Animator Animator;
    private Rigidbody rigidbodyBody;
    private Quaternion originCameraRotation;            //начальные оси поворота камеры
    private Vector3 originCameraPosition;               //начальные координаты камеры
    private float angleHorizontal;                      //углы поворота по осям
    private float angleVertical;
    private float diagonalMultiple = 1 / Mathf.Sqrt(2); //множитель ходьбы по диагонали
    private bool isRun = false;
    private bool isSit = false;

	private void Awake() {
        rigidbodyBody = GetComponent<Rigidbody>();
        originCameraRotation = mainCamera.transform.rotation;
        originCameraPosition = mainCamera.transform.localPosition;
        Animator = GetComponent<Animator>();
        currentState = PlayerState.WithoutWeapon;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        hands.SetActive(false);
    }

    private void FixedUpdate() {
        angleHorizontal += Input.GetAxis("Mouse X") * mouseSensitivity;
        angleVertical += Input.GetAxis("Mouse Y") * mouseSensitivity;
        angleVertical = Mathf.Clamp(angleVertical, -maxVerticalAngle, maxVerticalAngle);

        Quaternion rotationY = Quaternion.AngleAxis(angleHorizontal, Vector3.up);
        rigidbodyBody.MoveRotation(rotationY);
        rigidbodyBody.AddForce(GetVelocityChange(), ForceMode.VelocityChange);
	}

    private Vector3 GetVelocityChange() {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        if (targetVelocity.x != 0 && targetVelocity.z != 0) { //движение по диагонали
            targetVelocity *= diagonalMultiple;
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && !isSit) { //бег
            isRun = true;
            targetVelocity *= runMultiple;
        }
        else {
            isRun = false;
        }

        Vector3 velocityChange = (targetVelocity - rigidbodyBody.velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        return velocityChange;
    }

    private void LateUpdate() {
        Quaternion rotationY = Quaternion.AngleAxis(angleHorizontal, Vector3.up);
        Quaternion rotationX = Quaternion.AngleAxis(-angleVertical, Vector3.right);

        mainCamera.transform.rotation = originCameraRotation * rotationY * rotationX;

        if (Input.GetKey(KeyCode.LeftControl) && !isSit && !isRun) { //присест
            isSit = true;
            mainCamera.transform.localPosition = originCameraPosition - new Vector3(0, sitHeight, 0);
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && isSit) {
            isSit = false;
            mainCamera.transform.localPosition = originCameraPosition;
        }
    }

    private IEnumerator PickUpPM()
    {
        hands.SetActive(true);
        Animator.SetBool("FoundPm", true);
        
        currentState = PlayerState.PickingUpPM;
        yield return new WaitForSeconds(9.20f);
        currentState = PlayerState.WithPM;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Destroy(other.gameObject);
            StartCoroutine(PickUpPM());
        }
        
    }
}
