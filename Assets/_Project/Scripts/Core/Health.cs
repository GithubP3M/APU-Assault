using UnityEngine;
using UnityEngine.Events;

namespace APUAssault.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100f;

        public UnityEvent<float> onDamaged;
        public UnityEvent onDeath;

        public float Current { get; private set; }
        public float Max => maxHealth;
        public bool IsDead => Current <= 0f;

        void Awake() => Current = maxHealth;

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            Current = Mathf.Max(0f, Current - amount);
            onDamaged?.Invoke(amount);
            if (IsDead) onDeath?.Invoke();
        }

        public void Heal(float amount) => Current = Mathf.Min(maxHealth, Current + amount);
    }
}
