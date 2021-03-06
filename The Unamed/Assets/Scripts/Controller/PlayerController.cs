using UnityEngine;
using zheavy.Combat;

namespace zheavy.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 0f;
        [SerializeField] private float baseMoveSpeed = 5f;
        [SerializeField] private float speedBoost = 1.25f;
        [SerializeField] private float speedTransition = 5f;
        [SerializeField] private float rotationSpeed = 5f;

        [Header("Components")]
        private Animator anim;
        private Fighter fighter;
        private CharacterController controller;
        private InputMaster inputMaster;
        private Camera cam;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
            controller = GetComponent<CharacterController>();
            inputMaster = new InputMaster();
            cam = Camera.main;
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
            Move();
            Attack();
            WeaonSwap();
        }

        private void WeaonSwap()
        {
            if (inputMaster.Player.WeaponSlot1.ReadValue<float>() >= 1)
            {
                int inputKey = Mathf.RoundToInt(inputMaster.Player.WeaponSlot1.ReadValue<float>());
                Debug.LogError(inputKey);
                fighter.EquipWeapon(GetComponent<WeaponSlots>().SwitchWeapon(inputKey - 1));
            }
        }

        private void Move()
        {
            Vector2 movement = inputMaster.Player.Movement.ReadValue<Vector2>();
            if (movement.magnitude > 0
                && !fighter.IsAttacking())
            {
                // check for the camera offset isometric angle
                float cameraAngle = cam.GetComponent<Transform>().eulerAngles.y;
                float newAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraAngle;

                // smooth out the rotation
                Quaternion targetRotation = Quaternion.Euler(0, newAngle, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // move the player with proper speed and previously calculated angle
                Vector3 moveDirection = Quaternion.Euler(0, newAngle, 0) * Vector3.forward;
                SetMoveSpeed();
                controller.Move(moveDirection * Time.deltaTime * (moveSpeed));

                // set run animation while considering the speed factor
                anim.SetFloat("Run_Speed", 1);
                anim.speed = moveSpeed / baseMoveSpeed;
            }
            else anim.SetFloat("Run_Speed", 0);
        }

        private void SetMoveSpeed()
        {
            if (inputMaster.Player.SpeedBoost.ReadValue<float>() > 0)
            {
                // boost speed if booster input is held
                moveSpeed = Mathf.Lerp(moveSpeed, baseMoveSpeed * speedBoost, Time.deltaTime * speedTransition);
            }
            else
            {
                // base speed
                moveSpeed = Mathf.Lerp(moveSpeed, baseMoveSpeed, Time.deltaTime * speedTransition);
            }
        }

        private void Attack()
        {
            if (inputMaster.Player.Attack.ReadValue<float>() > 0
                && !fighter.IsAttacking())
            {
                // reset animator and player speed
                anim.speed = 1;
                moveSpeed = baseMoveSpeed;

                // start attack
                anim.SetTrigger("Attack");
                anim.SetInteger("Random_Attack", Random.Range(0, 2));
                StartCoroutine(fighter.Attack());
            }
        }

        public void AnimStopAttack()
        {
            anim.ResetTrigger("Attack");

        }
    }
}


// not using this section - ONLY WASD movement for now - no mouse moveme
//private void SetLookRotationToMouse()
//{
//    // instantly rotate player to mouse pointer
//    Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
//    RaycastHit hit;

//    if (Physics.Raycast(ray, out hit))
//    {
//        Vector3 rotationPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
//        transform.LookAt(rotationPoint);
//    }
//}