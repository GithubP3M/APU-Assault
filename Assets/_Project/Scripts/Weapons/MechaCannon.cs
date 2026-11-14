using System.Collections.Generic;
using APUAssault.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace APUAssault.Weapons
{
    public class MechaCannon : MonoBehaviour
    {
        public enum Hand { Left, Right }

        [Header("References")]
        [SerializeField] Hand hand = Hand.Right;
        [SerializeField] Transform muzzle;
        [SerializeField] LayerMask hitMask = ~0;
        [SerializeField] InputActionReference primaryOverride;
        [SerializeField] InputActionReference secondaryOverride;

        [Header("Primary fire (trigger)")]
        [SerializeField] float primaryDamage = 20f;
        [SerializeField] float primaryCooldown = 0.15f;

        [Header("Secondary fire (grip)")]
        [SerializeField] float secondaryDamage = 80f;
        [SerializeField] float secondaryRadius = 3f;
        [SerializeField] float secondaryCooldown = 2f;

        [Header("Common")]
        [SerializeField] float range = 100f;
        [SerializeField] float pressThreshold = 0.5f;

        InputAction primary, secondary;
        float nextPrimary, nextSecondary;

        string HandUsage => hand == Hand.Left ? "LeftHand" : "RightHand";
        Transform Origin => muzzle != null ? muzzle : transform;

        void OnEnable()
        {
            primary = primaryOverride != null ? primaryOverride.action : new InputAction("Primary", InputActionType.Value, $"<XRController>{{{HandUsage}}}/trigger");
            secondary = secondaryOverride != null ? secondaryOverride.action : new InputAction("Secondary", InputActionType.Value, $"<XRController>{{{HandUsage}}}/grip");
            primary.Enable();
            secondary.Enable();
        }

        void OnDisable()
        {
            primary?.Disable();
            secondary?.Disable();
            if (primaryOverride == null) primary?.Dispose();
            if (secondaryOverride == null) secondary?.Dispose();
        }

        void Update()
        {
            if (primary.ReadValue<float>() >= pressThreshold && Time.time >= nextPrimary)
            {
                nextPrimary = Time.time + primaryCooldown;
                FirePrimary();
            }
            if (secondary.ReadValue<float>() >= pressThreshold && Time.time >= nextSecondary)
            {
                nextSecondary = Time.time + secondaryCooldown;
                FireSecondary();
            }
        }

        bool Cast(out RaycastHit hit) =>
            Physics.Raycast(Origin.position, Origin.forward, out hit, range, hitMask, QueryTriggerInteraction.Ignore);

        void FirePrimary()
        {
            if (Cast(out var hit) && hit.collider.GetComponentInParent<Health>() is Health health)
                health.TakeDamage(primaryDamage);
        }

        void FireSecondary()
        {
            var point = Cast(out var hit) ? hit.point : Origin.position + Origin.forward * range;
            var damaged = new HashSet<Health>();
            foreach (var col in Physics.OverlapSphere(point, secondaryRadius, hitMask, QueryTriggerInteraction.Ignore))
                if (col.GetComponentInParent<Health>() is Health health && !transform.IsChildOf(health.transform) && damaged.Add(health))
                    health.TakeDamage(secondaryDamage);
        }
    }
}
