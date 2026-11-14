using UnityEngine;

namespace APUAssault.Weapons
{
    public class MechaArm : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform controller;
        [SerializeField] Transform pivot;
        [SerializeField] Transform trackingRoot;
        [SerializeField] Transform mechaAnchor;

        [Header("Mapping")]
        [SerializeField] float positionScale = 3f;
        [SerializeField] Vector3 shoulderOffset = new Vector3(1.5f, 0f, 0.5f);
        [SerializeField] Vector3 rotationOffset;

        void OnEnable() => Application.onBeforeRender += Apply;
        void OnDisable() => Application.onBeforeRender -= Apply;
        void LateUpdate() => Apply();

        void Apply()
        {
            if (controller == null || pivot == null || trackingRoot == null || mechaAnchor == null) return;

            var handFromHead = trackingRoot.InverseTransformVector(controller.position - pivot.position);
            transform.position = mechaAnchor.position + mechaAnchor.TransformVector(handFromHead * positionScale + shoulderOffset);

            var handRotation = Quaternion.Inverse(trackingRoot.rotation) * controller.rotation;
            transform.rotation = mechaAnchor.rotation * handRotation * Quaternion.Euler(rotationOffset);
        }
    }
}
