using APUAssault.Core;
using APUAssault.Enemies;
using APUAssault.Weapons;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace APUAssault.Editor
{
    public static class StartSceneBuilder
    {
        const string RigPath = "Assets/Samples/XR Interaction Toolkit/3.7.0/Starter Assets/Prefabs/XR Origin (XR Rig).prefab";
        const string ScenePath = "Assets/_Project/Scenes/Main.unity";

        [MenuItem("APU Assault/Create Start Scene")]
        public static void Build()
        {
            var rigPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RigPath);
            if (rigPrefab == null)
            {
                EditorUtility.DisplayDialog("APU Assault", "XR Origin prefab not found:\n" + RigPath, "OK");
                return;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var lightGo = new GameObject("Directional Light");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            new GameObject("XR Interaction Manager").AddComponent<XRInteractionManager>();
            new GameObject("Game Manager").AddComponent<GameManager>();

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(5f, 1f, 5f);

            var rig = (GameObject)PrefabUtility.InstantiatePrefab(rigPrefab, scene);
            rig.AddComponent<Health>();

            BuildGun();
            BuildEnemy(new Vector3(0f, 1f, 6f));

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            Debug.Log("APU Assault: start scene created at " + ScenePath);
        }

        static void BuildGun()
        {
            var gun = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gun.name = "Gun";
            gun.transform.position = new Vector3(0f, 1f, 0.6f);
            gun.transform.localScale = new Vector3(0.05f, 0.1f, 0.2f);
            gun.AddComponent<Rigidbody>();
            gun.AddComponent<XRGrabInteractable>();

            var muzzle = new GameObject("Muzzle").transform;
            muzzle.SetParent(gun.transform, false);
            muzzle.localPosition = new Vector3(0f, 0f, 0.6f);

            var comp = gun.AddComponent<Gun>();
            var so = new SerializedObject(comp);
            so.FindProperty("muzzle").objectReferenceValue = muzzle;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void BuildEnemy(Vector3 position)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy";
            enemy.transform.position = position;
            enemy.AddComponent<Health>();
            enemy.AddComponent<EnemyChaser>();
        }
    }
}
