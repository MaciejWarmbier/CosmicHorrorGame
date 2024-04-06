using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpriteTurnController : MonoBehaviour
{
    private void FixedUpdate()
    {
        var lookPos = Camera.main.gameObject.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

    }
}
