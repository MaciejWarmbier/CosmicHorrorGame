using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Action<Collider> HandleOnTriggerStay;
    public BoxCollider colliderObj;

    private void OnTriggerStay(Collider collision)
    {
        HandleOnTriggerStay?.Invoke(collision);
    }

    public void SetCollider(bool set)
    {
        colliderObj.enabled = set;
    }
}
