using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReuseCollisonCallbacksChecker : MonoBehaviour
{
    [SerializeField] private Toggle _reuseCollisionCallbackToggle = null;

    private static readonly string ColliderRootName = "Colliders";
    private static readonly string ColliderNameFormat = "Collider_{0}";
    private static readonly float GroundOneSide = 10f;
    private static readonly float GroundHeight = 0f;

    [SerializeField] private int _groundSize = 3;

    [SerializeField] private Transform _levelRoot = null;

    [SerializeField] private TMP_InputField _colliderCountInput = null;
    [SerializeField] private Button _spawnButton = null;
    [SerializeField] private Button _shotButton = null;
    [SerializeField] private Button _sleepButton = null;

    [SerializeField] private GameObject _groundTemplate = null;
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

    private Bounds SpawnArea => new Bounds(new Vector3(0f, 4.5f, 0f), new Vector3(4f * _groundSize, 9f, 4f * _groundSize));
    private void SpawnColliders(int count, Transform root)
    {
        Bounds area = SpawnArea;
        for (var i = 0; i < count; ++i)
        {
            var collider = ((GameObject)UnityEngine.Object.Instantiate(_sphereColliderTemplate, root, false)).transform;
            collider.name = string.Format(ColliderNameFormat, i + 1);
            collider.position = new Vector3(
                UnityEngine.Random.Range(area.min.x, area.max.x),
                0.5f,
                UnityEngine.Random.Range(area.min.z, area.max.z)
            );
            collider.rotation = UnityEngine.Random.rotationUniform;

            var rigidbody = collider.gameObject.AddComponent<Rigidbody>();
            rigidbody.Sleep();

            collider.gameObject.AddComponent<ReuseCollisionCallbackListener>();
        }
    }

    private void InitializeUI()
    {
        if (_reuseCollisionCallbackToggle != null)
        {
            _reuseCollisionCallbackToggle.isOn = Physics.reuseCollisionCallbacks;
            _reuseCollisionCallbackToggle.onValueChanged.AddListener((isOn) =>
            {
                Physics.reuseCollisionCallbacks = isOn;
            });
        }

        if (_spawnButton != null)
        {
            _spawnButton.onClick.AddListener(() => 
            {
                if (_colliderCountInput == null)
                {
                    return;
                }

                if (_levelRoot == null)
                {
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

                SpawnColliders(count, root);
            });
        }

        if (_shotButton != null)
        {
            _shotButton.onClick.AddListener(() => 
            {
                if (_levelRoot == null)
                {
                    return;
                }

                foreach (var rigidbody in _levelRoot.GetComponentsInChildren<Rigidbody>(true))
                {
                    rigidbody.velocity = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(30f, 50f);
                }

                Debug.Break();
            });
        }

        if (_sleepButton != null)
        {
            _sleepButton.onClick.AddListener(() => 
            {
                if (_levelRoot == null)
                {
                    return;
                }

                foreach (var rigidbody in _levelRoot.GetComponentsInChildren<Rigidbody>(true))
                {
                    rigidbody.Sleep();;
                }
            });
        }

    }
}
