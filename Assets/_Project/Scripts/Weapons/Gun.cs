using APUAssault.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace APUAssault.Weapons
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class Gun : MonoBehaviour
    {
        [SerializeField] Transform muzzle;
        [SerializeField] float damage = 25f;
        [SerializeField] float range = 100f;
        [SerializeField] float fireCooldown = 0.15f;
        [SerializeField] LayerMask hitMask = ~0;
        [SerializeField] AudioSource shotAudio;

        XRGrabInteractable grab;
        float nextFireTime;

        void Awake() => grab = GetComponent<XRGrabInteractable>();
        void OnEnable() => grab.activated.AddListener(OnActivated);
        void OnDisable() => grab.activated.RemoveListener(OnActivated);

        void OnActivated(ActivateEventArgs args) => Fire();

        public void Fire()
        {
            if (Time.time < nextFireTime) return;
            nextFireTime = Time.time + fireCooldown;

            var origin = muzzle != null ? muzzle : transform;
            if (shotAudio != null) shotAudio.Play();

            if (Physics.Raycast(origin.position, origin.forward, out var hit, range, hitMask, QueryTriggerInteraction.Ignore)
                && hit.collider.GetComponentInParent<Health>() is Health health)
                health.TakeDamage(damage);
        }
    }
}
