using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float speedBoost = 1.25f;
    [SerializeField] private float speedTransition = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Components")]
    [SerializeField] private Animator anim;


    private CharacterController controller;
    private InputMaster inputMaster;
    private Camera cam;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputMaster = new InputMaster();
        cam = Camera.main;

        inputMaster.Player.Shoot.performed += _ => Shoot();
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    private void Start()
    {
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        Vector2 movement = inputMaster.Player.Movement.ReadValue<Vector2>();
        Move(movement);
    }

    private void Move(Vector2 movement)
    {
        Vector3 inputDirection = new Vector3(movement.x, 0, movement.y);

        if (inputDirection.magnitude > 0)
        {
            float cameraAngle = cam.GetComponent<Transform>().eulerAngles.y;
            float newAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraAngle;
            Quaternion targetRotation = Quaternion.Euler(0, newAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDirection = Quaternion.Euler(0, newAngle, 0) * Vector3.forward;

            SetSpeed();

            controller.Move(moveDirection * Time.deltaTime * (moveSpeed));
            anim.SetBool("Run", true);
            anim.speed = moveSpeed / baseMoveSpeed;
        }
        else anim.SetBool("Run", false);
    }

    private void SetSpeed()
    {
        if (inputMaster.Player.SpeedBoost.ReadValue<float>() > 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed ,baseMoveSpeed * speedBoost, Time.deltaTime * speedTransition) ;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, baseMoveSpeed, Time.deltaTime * speedTransition);
        }
    }

    private void Shoot()
    {
        SetLookRotationToMouse();
    }

    private void SetLookRotationToMouse()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 rotationPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(rotationPoint);
        }
    }
}

