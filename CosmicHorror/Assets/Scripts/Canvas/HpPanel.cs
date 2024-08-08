using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpPanel : MonoBehaviour
{
    public static HpPanel HpPanelInstance = null;

    [SerializeField] Slider slider;
    [SerializeField] List<GameObject> stressBars;

    void Awake()
    {
        HpPanelInstance = this;
        SetStress(0);
    }

    public void SetStress(int stress)
    {
        for(int i = 0; i < stress; i++)
        {
            stressBars[i].SetActive(true);
        }

        for (int i = stress; i < stressBars.Count; i++)
        {
            stressBars[i].SetActive(false);
        }
    }

    public void SetHp(long hpNow, int stress)
    {
        SetStress(stress);
        slider.value = hpNow;
    }

    public void Setup(long hpNow, long hpMax, int stress)
    {
        slider.maxValue = hpMax;
        if(hpNow > hpMax - stress * 10)
        {
            hpNow = hpMax - stress * 10;
        }

        slider.value = hpNow;   
    }
}
