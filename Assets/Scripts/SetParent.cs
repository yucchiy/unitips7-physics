using UnityEngine;

[ExecuteInEditMode]
public class SetParent : MonoBehaviour
{
    [SerializeField] private Transform _parent = null;
    [SerializeField] private Vector3 _offset = Vector3.zero;

    private RectTransform _rectTransform = null;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (_parent == null || _rectTransform == null)
        {
            return;
        }

        _rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(_parent.transform.position + _offset) -
            (new Vector3(Screen.width, Screen.height)) / 2f;
    }
}