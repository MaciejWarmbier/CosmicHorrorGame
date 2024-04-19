using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public static PlayerStatistics PlayerStatisticslInstance = null;

    public int HealthPoints {  get; private set; }
    public int MaxHealthPoints {  get; private set; }
    public int StressLevel {  get; private set; }

    void Start()
    {
        PlayerStatisticslInstance = this;
        MaxHealthPoints = 100;
        HealthPoints = MaxHealthPoints;

        HpPanel.HpPanelInstance.Setup(HealthPoints, MaxHealthPoints);
    }

    public void ChangeHealth(int health)
    {
        HealthPoints += health;
        HpPanel.HpPanelInstance.SetHp(HealthPoints);
    }
}
