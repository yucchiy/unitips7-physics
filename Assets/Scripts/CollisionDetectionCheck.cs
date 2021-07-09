using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetectionCheck : MonoBehaviour
{
    [SerializeField] Rigidbody[] _rigidbodies = null;
    private Vector3[] _startPoints = null;

    [SerializeField] private TMP_InputField _forceInput = null;
    [SerializeField] private Button _shotButton = null;
    [SerializeField] private Button _resetButton  = null;

    void Start()
    {
        InitializeStartPoint();
        InitializeUI();
    }

    private void InitializeStartPoint()
    {
        if (_rigidbodies != null)
        {
            _startPoints = new Vector3[_rigidbodies.Length];
            for (var i = 0; i < _startPoints.Length; ++i)
            {
                _startPoints[i] = _rigidbodies[i].transform.position;
            }
        }
    }

    private void InitializeUI()
    {
        if (_shotButton != null)
        {
            _shotButton.onClick.AddListener(() =>
            {
                if (_rigidbodies == null  || _forceInput == null)
                {
                    return;
                }

                if (!Int32.TryParse(_forceInput.text, out var force))
                {
                    return;
                }

                foreach (var rigidbody in _rigidbodies)
                {
                    rigidbody.velocity = new Vector3(-force, 0f, 0f);
                }

                Debug.Break();
            });
        }

        if (_resetButton != null)
        {
            _resetButton.onClick.AddListener(() =>
            {
                if (_rigidbodies == null  || _startPoints == null)
                {
                    return;
                }

                for (var i = 0; i < _startPoints.Length; ++i)
                {
                    var rigidbody = _rigidbodies[i];
                    rigidbody.transform.position = _startPoints[i];
                    rigidbody.transform.rotation = Quaternion.identity;
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.Sleep();
                }
            });
        }
    }
}
