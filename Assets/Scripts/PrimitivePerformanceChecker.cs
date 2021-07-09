using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrimitivePerformanceChecker : MonoBehaviour
{
    private static readonly string ColliderRootName = "Colliders";
    private static readonly string ColliderNameFormat = "Collider_{0}";
    private static readonly string ColliderALayerName = "PrimitiveCheck_A";
    private static readonly string ColliderBLayerName = "PrimitiveCheck_B";
    private static readonly float GroundOneSide = 10f;
    private static readonly float GroundHeight = 0f;

    [SerializeField] private int _groundSize = 3;

    [SerializeField] private Transform _levelRoot = null;

    [SerializeField] private TMP_Dropdown _colliderTypeSelectorA = null;
    [SerializeField] private TMP_Dropdown _colliderTypeSelectorB = null;
    [SerializeField] private TMP_InputField _colliderCountInput = null;
    [SerializeField] private Button _spawnButton = null;

    [SerializeField] private GameObject _groundTemplate = null;
    [SerializeField] private GameObject _boxColliderTemplate = null;
    [SerializeField] private GameObject _capsuleColliderTemplate = null;
    [SerializeField] private GameObject _sphereColliderTemplate = null;

    void Start()
    {
        InitializeGround();
        InitializeUI();
    }

    private void InitializeGround()
    {
        if (_groundTemplate == null)
        {
            Debug.LogError("ground template not set.", this);
            return;
        }

        for (var i = 0; i < _groundSize; ++i)
        {
            for (var j = 0; j < _groundSize; ++j)
            {
                var ground = ((GameObject)UnityEngine.Object.Instantiate(_groundTemplate, _levelRoot, false)).transform;
                ground.localPosition = new Vector3(
                    (_groundSize / 2 - i) * GroundOneSide,
                    0f,
                    (_groundSize / 2 - j) * GroundOneSide 
                );
            }
        }
    }

    private void InitializeUI()
    {
        if (_spawnButton != null)
        {
            _spawnButton.onClick.AddListener(() => 
            {
                if (_colliderTypeSelectorA == null || _colliderTypeSelectorB == null)
                {
                    Debug.LogError($"collider type selector not set.", this);
                    return;
                }

                if (_colliderCountInput == null)
                {
                    Debug.LogError($"collider coutn input not set.", this);
                    return;
                }

                if (_levelRoot == null)
                {
                    Debug.LogError("level root not set.", this);
                    return;
                }

                var root = _levelRoot.Find(ColliderRootName);
                if (root != null)
                {
                    GameObject.Destroy(root.gameObject);
                }
                root = (new GameObject(ColliderRootName)).transform;
                root.SetParent(_levelRoot, false);

                if (!Int32.TryParse(_colliderCountInput.text, out var count))
                {
                    Debug.LogError($"failed to parse {_colliderCountInput.text} as Int32.", this);
                    return;
                }

                var layerA = LayerMask.NameToLayer(ColliderALayerName);
                switch (_colliderTypeSelectorA.options[_colliderTypeSelectorA.value].text)
                {
                    case "Box":
                        SpawnColliders(_boxColliderTemplate, count, layerA, root);
                        break;
                    case "Capsule":
                        SpawnColliders(_capsuleColliderTemplate, count, layerA, root);
                        break;
                    case "Sphere":
                        SpawnColliders(_sphereColliderTemplate, count, layerA, root);
                        break;
                }

                var layerB = LayerMask.NameToLayer(ColliderBLayerName);
                switch (_colliderTypeSelectorB.options[_colliderTypeSelectorB.value].text)
                {
                    case "Box":
                        SpawnColliders(_boxColliderTemplate, count, layerB, root);
                        break;
                    case "Capsule":
                        SpawnColliders(_capsuleColliderTemplate, count, layerB, root);
                        break;
                    case "Sphere":
                        SpawnColliders(_sphereColliderTemplate, count, layerB, root);
                        break;
                }
            });
        }
    }

    private Bounds SpawnArea => new Bounds(new Vector3(0f, 4.5f, 0f), new Vector3(4f * _groundSize, 9f, 4f * _groundSize));

    private void SpawnColliders(GameObject template, int count, int layer, Transform root)
    {
        Bounds area = SpawnArea;
        for (var i = 0; i < count; ++i)
        {
            var collider = ((GameObject)UnityEngine.Object.Instantiate(template, root, false)).transform;
            collider.name = string.Format(ColliderNameFormat, i + 1);
            collider.localPosition = new Vector3(
                UnityEngine.Random.Range(area.min.x, area.max.x),
                UnityEngine.Random.Range(area.min.y, area.max.y),
                UnityEngine.Random.Range(area.min.z, area.max.z)
            );
            collider.localRotation = UnityEngine.Random.rotationUniform;
            collider.gameObject.layer = layer;
        }
    }
}
