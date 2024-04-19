using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpPanel : MonoBehaviour
{
    public static HpPanel HpPanelInstance = null;

    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Slider slider;

    void Start()
    {
        HpPanelInstance = this;
    }

    public void SetHp(int hpNow)
    {
        slider.value = hpNow;
        hpText.text = $"{hpNow}/{slider.maxValue}";
    }

    public void Setup(int hpNow, int hpMax)
    {
        slider.maxValue = hpMax;
        slider.value = hpNow;

        hpText.text = $"{hpNow}/{hpMax}";
    }
}
