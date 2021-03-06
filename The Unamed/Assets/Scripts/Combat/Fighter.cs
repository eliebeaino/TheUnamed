using System.Collections;
using UnityEngine;

namespace zheavy.Combat
{
    public class Fighter : MonoBehaviour
    {
        [Header("Componenets")]
        // HEALTH = target
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeapon = null;

        public WeaponConfig currentWeaponConfig; // TEMP public
        public Weapon currentWeapon; // TEMP public
        Animator anim;

        [Header("Stats")]
        [SerializeField] float attackSpeed = 1f;

        private float timeSinceLastAttack = Mathf.Infinity; // allows attack directly upon first combat - TEMP - may change

        public bool isAttacking = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            currentWeapon = AttachWeapon(defaultWeapon);
            currentWeaponConfig = defaultWeapon;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            // TODO animation
        }

        public IEnumerator Attack()
        {
            //transform.LookAt(target);
            if (timeSinceLastAttack >= attackSpeed)
            {
                isAttacking = true;
                timeSinceLastAttack = 0;
                yield return new WaitForSeconds(attackSpeed);
                isAttacking = false;
            }
        }

        public void EquipWeapon(WeaponConfig newWeapon)
        {
            currentWeaponConfig = newWeapon;
            currentWeapon = AttachWeapon(currentWeaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig newWeapon)
        {
            return newWeapon.SpawnWpn(rightHandTransform, leftHandTransform, anim);
        }

        public void MeleeHit()
        {
            // ANIMATION EVENT
            Debug.Log("I swung my arms!");

            Collider[] cols = Physics.OverlapSphere(transform.position, currentWeaponConfig.GetWeaponRange());
            foreach (Collider col in cols)
            {
                if (!col) continue;
                if (col.gameObject == this.gameObject) continue;
                if (col.gameObject.layer == LayerMask.NameToLayer("Character"))
                {
                    col.gameObject.GetComponent<Enemy>().TakeDamage(currentWeaponConfig.GetWeaponPhysicalDamage());
                    Debug.Log("I hit the target!");
                }
            }
        }

        public bool IsAttacking()
        {
            return isAttacking;
        }
    }
}