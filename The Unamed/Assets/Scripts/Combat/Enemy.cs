using UnityEngine;

namespace zheavy.Combat
{
    public class Enemy : MonoBehaviour
    {
        [Header(" Stats")]
        [SerializeField] float maxHealth = 10f;
        [SerializeField] float power, toughness;

        public float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (currentHealth > 0)
            {
                Debug.Log(currentHealth - damage);
                currentHealth -= damage;
            }
            //if (currentHealth <= 0)
            //{
            //    //play death animation
            //}
        }
    }
}