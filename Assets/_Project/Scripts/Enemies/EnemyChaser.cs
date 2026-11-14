using APUAssault.Core;
using UnityEngine;

namespace APUAssault.Enemies
{
    [RequireComponent(typeof(Health))]
    public class EnemyChaser : MonoBehaviour
    {
        [SerializeField] float speed = 1.5f;
        [SerializeField] float attackRange = 1f;
        [SerializeField] float attackDamage = 10f;
        [SerializeField] float attackCooldown = 1f;
        [SerializeField] int scoreValue = 10;

        Health health;
        Transform target;
        float nextAttackTime;

        void Awake() => health = GetComponent<Health>();
        void OnEnable() => health.onDeath.AddListener(OnDeath);
        void OnDisable() => health.onDeath.RemoveListener(OnDeath);

        void Update()
        {
            if (target == null)
            {
                var cam = Camera.main;
                if (cam == null) return;
                target = cam.transform;
            }

            var toTarget = target.position - transform.position;
            toTarget.y = 0f;
            var distance = toTarget.magnitude;
            if (distance > 0.01f) transform.rotation = Quaternion.LookRotation(toTarget);

            if (distance > attackRange)
            {
                transform.position += toTarget.normalized * (speed * Time.deltaTime);
            }
            else if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                var playerHealth = target.GetComponentInParent<Health>();
                if (playerHealth != null) playerHealth.TakeDamage(attackDamage);
            }
        }

        void OnDeath()
        {
            if (GameManager.Instance != null) GameManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
