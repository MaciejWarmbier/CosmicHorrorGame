using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    private bool wasHit = false;


    public void OnRaycastHit()
    {
        Debug.Log("You hitted object");

        var meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material.color = wasHit ? Color.green : Color.red;
        
        wasHit = !wasHit;
    }
}
