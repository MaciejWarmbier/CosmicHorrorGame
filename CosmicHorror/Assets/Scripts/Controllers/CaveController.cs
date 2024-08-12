using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CaveController : MonoBehaviour
{
    public static CaveController CaveControllerInstance = null;

    [SerializeField] AudioSource audio;
    [SerializeField] Material shader;
    [SerializeField] float firstCooldown;
    [SerializeField] float noamralCoooldown;
    [SerializeField] float shaderTime;

    private string _shaderTime = "_animationTime";

    private void Awake()
    {
        CaveControllerInstance = this;
    }

    private void Start()
    {
        ChangeShaderTime(false);
    }

    public void ChangeShaderTime(bool activate)
    {
        float time = activate ? 1.5f : 0;
        shader.SetFloat(_shaderTime, time);
    }
     
    public void PutShaderOnFirstCooldown()
    {
        StartCoroutine(PutShaderOnCooldowneCooldown(firstCooldown));
    }

    private IEnumerator PutShaderOnCooldowneCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        ChangeShaderTime(true);
        audio.Play();
        PlayerStatistics.PlayerStatisticslInstance.ChangeStress(1);

        yield return new WaitForSeconds(shaderTime);

        ChangeShaderTime(false);

        StartCoroutine(PutShaderOnCooldowneCooldown(noamralCoooldown));
    }
}
