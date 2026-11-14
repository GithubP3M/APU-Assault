using APUAssault.Weapons;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace APUAssault.Editor
{
    public static class CannonRigAttacher
    {
        const string CannonPath = "Assets/Prefabs/Cannon.prefab";
        const string RigName = "XR Origin (XR Rig)";

        [MenuItem("APU Assault/Setup Mecha Cannons")]
        public static void Setup()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CannonPath);
            var rig = GameObject.Find(RigName);
            var cam = Camera.main;
            if (prefab == null || rig == null || cam == null)
            {
                EditorUtility.DisplayDialog("APU Assault", "Missing Cannon prefab, '" + RigName + "' or a MainCamera in the active scene.", "OK");
                return;
            }
            if (rig.GetComponentInChildren<MechaArm>(true) != null)
            {
                Debug.Log("APU Assault: mecha cannons already set up.");
                return;
            }

            var mecha = new GameObject("Mecha");
            mecha.transform.SetParent(rig.transform, false);
            Undo.RegisterCreatedObjectUndo(mecha, "Setup Mecha Cannons");

            SetupArm(prefab, mecha.transform, rig.transform, cam.transform, "Left Controller", MechaCannon.Hand.Left, -1.5f);
            SetupArm(prefab, mecha.transform, rig.transform, cam.transform, "Right Controller", MechaCannon.Hand.Right, 1.5f);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("APU Assault/Show Controller Models")]
        public static void ShowControllerModels()
        {
            foreach (var name in new[] { "Left Controller", "Right Controller" })
            {
                var controller = GameObject.Find(name);
                if (controller == null) continue;
                foreach (var r in controller.GetComponentsInChildren<Renderer>(true))
                    if (PrefabUtility.IsPartOfPrefabInstance(r))
                        PrefabUtility.RevertObjectOverride(r, InteractionMode.AutomatedAction);
            }
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        static void SetupArm(GameObject prefab, Transform mecha, Transform rig, Transform head, string controllerName, MechaCannon.Hand hand, float shoulderX)
        {
            var controller = GameObject.Find(controllerName);
            if (controller == null)
            {
                Debug.LogWarning("APU Assault: '" + controllerName + "' not found in the active scene.");
                return;
            }

            var cannon = (GameObject)PrefabUtility.InstantiatePrefab(prefab, mecha);
            var cannonComp = cannon.AddComponent<MechaCannon>();
            var cannonSo = new SerializedObject(cannonComp);
            cannonSo.FindProperty("hand").enumValueIndex = (int)hand;
            cannonSo.ApplyModifiedPropertiesWithoutUndo();

            var arm = cannon.AddComponent<MechaArm>();
            var armSo = new SerializedObject(arm);
            armSo.FindProperty("controller").objectReferenceValue = controller.transform;
            armSo.FindProperty("pivot").objectReferenceValue = head;
            armSo.FindProperty("trackingRoot").objectReferenceValue = rig;
            armSo.FindProperty("mechaAnchor").objectReferenceValue = mecha;
            armSo.FindProperty("shoulderOffset").vector3Value = new Vector3(shoulderX, 1.5f, 0.5f);
            armSo.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
