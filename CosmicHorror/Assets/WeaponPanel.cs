using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    public static WeaponPanel WeaponPanelInstance;

    [Header("Revolver")]
    [SerializeField] List<Image> revolverBullets;
    [SerializeField] Sprite normalBullet;
    [SerializeField] Sprite usedBullet;
    [SerializeField] GameObject revolverObject;
    [SerializeField] TextMeshProUGUI bulletNumber;

    [Header("Machete")]
    [SerializeField] GameObject macheteObject;

    // Start is called before the first frame update
    void Start()
    {
        WeaponPanelInstance = this;
        SetRevolverAmmo();
        PlayerStatistics.PlayerStatisticslInstance.OnRevolverUsed += SetRevolverAmmo;
    }

    private void SetRevolverAmmo()
    {
        for(int i= 0; i < PlayerStatistics.PlayerStatisticslInstance.RevolverLoadedAmmo; i++)
        {
            revolverBullets[i].sprite = normalBullet;
        }

        for (int i = PlayerStatistics.PlayerStatisticslInstance.RevolverLoadedAmmo; i < revolverBullets.Count; i++)
        {
            revolverBullets[i].sprite = usedBullet;
        }

        bulletNumber.text = $"x{PlayerStatistics.PlayerStatisticslInstance.RevolverAmmo}";
    }

    public void OnWeaponSwap(WeaponEnum weapon)
    {
        if(weapon == WeaponEnum.Machete)
        {
            macheteObject.SetActive(true);
            revolverObject.SetActive(false);
        }
        else
        {
            macheteObject.SetActive(false);
            revolverObject.SetActive(true);
        }
    }

    public enum WeaponEnum
    {
        Machete,
        Revolver
    }
}
