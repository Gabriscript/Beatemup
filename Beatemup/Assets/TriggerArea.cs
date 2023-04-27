using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    public enum TriggerType { All, OneObject, Tag, Layer };
    [Header("What to react to")]
    public TriggerType reactTo;
    public GameObject target;
    public new string tag;
    public LayerMask layer;
    [Header("Reactions")]
    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent onExit;

    bool ValidTarget(Collider other) {
        if (reactTo == TriggerType.All)
            return true;
        else if (reactTo == TriggerType.OneObject)
            return other.gameObject == target;
        else if (reactTo == TriggerType.Tag)
            return other.tag == tag;
        else
            return ((1 << other.gameObject.layer) & layer.value) != 0;
    }

    void OnTriggerEnter(Collider other) {
        if (ValidTarget(other)) {
            onEnter.Invoke();
        }
    }

    void OnTriggerStay(Collider other) {
        if (ValidTarget(other)) {
            onStay.Invoke();
        }
    }

    void OnTriggerExit(Collider other) {
        if (ValidTarget(other)) {
            onExit.Invoke();
        }
    }
}
