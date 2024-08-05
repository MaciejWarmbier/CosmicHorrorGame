using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] AudioSource doorAudio;

    private void OnEnable()
    {
        doorAudio.Play();
    }

    private void OnDisable()
    {
        doorAudio.Play();
    }
}
