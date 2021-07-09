using System;
using UnityEngine;
using TMPro;

public class FixedUpdateTimestepChecker : MonoBehaviour
{
    /// <summary>
    /// そのフレームで呼び出されたFixedUpdateの回数を記録します
    /// </summary>
    private int _fixedUpdateCount = 0;

    /// <summary>
    /// UpdateでのdeltaTime、つまりゲームにおけるそのフレームでの経過時間
    /// </summary>
    private float _updateDeltaTime = 0f;

    /// <summary>
    /// フレーム内でFixedUpdateが呼び出されたかどうかを記録します
    /// </summary>
    private bool _isCalledFixedUpdateInThisFrame = false;

    [SerializeField] TextMeshProUGUI _fixedUpdateCountText = null;
    [SerializeField] TextMeshProUGUI _deltaTimeText = null;
    [SerializeField] TextMeshProUGUI _fixedTimestepText = null;

    void Start()
    {
        Reset();
    }

    void FixedUpdate()
    {
        _fixedUpdateCount++;
    }

    void Update()
    {
        _updateDeltaTime = Time.deltaTime;

        UpdateUI();
        Reset();
    }

    /// <summary>
    /// フレームリセット処理を行います
    /// </summary>
    private void Reset()
    {
        _fixedUpdateCount = 0;
    }

    /// <summary>
    /// UIを更新します
    /// </summary>
    private void UpdateUI()
    {
        if (_fixedUpdateCountText != null)
        {
            _fixedUpdateCountText.text = string.Format("FixedUpdateCount: {0}", _fixedUpdateCount);
        }

        if (_deltaTimeText != null)
        {
            _deltaTimeText.text = string.Format("DeltaTime: {0:0.000} sec", _updateDeltaTime);
        }

        if (_fixedTimestepText != null)
        {
            _fixedTimestepText.text = string.Format("FixedTimestep: {0:0.000} sec", Time.fixedDeltaTime);
        }
    }
}
