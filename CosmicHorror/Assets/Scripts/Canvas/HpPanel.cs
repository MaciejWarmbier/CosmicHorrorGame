using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpPanel : MonoBehaviour
{
    public static HpPanel HpPanelInstance = null;

    //[SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Slider slider;

    void Awake()
    {
        HpPanelInstance = this;
    }

    public void SetHp(long hpNow)
    {
        slider.value = hpNow;
       // hpText.text = $"{hpNow}/{slider.maxValue}";
    }

    public void Setup(long hpNow, long hpMax)
    {
        slider.maxValue = hpMax;
        slider.value = hpNow;

      //  hpText.text = $"{hpNow}/{hpMax}";
    }
}
