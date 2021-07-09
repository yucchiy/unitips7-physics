using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ReuseCollisionCallbackListener : MonoBehaviour
{
    private Collider _collider;

    void Start()
    {
        _collider = gameObject.GetComponent<Collider>();
        _collider.isTrigger = false;
    }
    void OnCollisionEnter(Collision other)
    {
        //Debug.Log($"OnCollisionEnter(other hashcode = {other.GetHashCode()})");
    }

    void OnCollisionStay(Collision other)
    {
        Debug.Log($"[{name}] OnCollisionStay(other({other.gameObject.name}), hashcode = {other.GetHashCode()})");
    }

    void OnCollisionExit(Collision other)
    {
        //Debug.Log($"OnCollisionExit(other hashcode = {other.GetHashCode()})");
    }
}

