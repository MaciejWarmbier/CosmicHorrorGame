using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

public class PlayerStatistics : MonoBehaviour
{
    public Action OnRevolverUsed;

    public static PlayerStatistics PlayerStatisticslInstance = null;

    public long HealthPoints {  get; private set; }
    public long MaxHealthPoints {  get; private set; }
    public int MaxStress {  get; private set; }
    public int StressLevel {  get; private set; }

    [Header("Revolver")]
    public int RevolverAmmo;
    public int RevolverLoadedAmmo;
    public int RevolverMaxAmmo;

    bool _isSceneLoading = false;

    private void Awake()
    {
        PlayerStatisticslInstance = this;

        MaxHealthPoints = 100;
        MaxStress = 9;
        HealthPoints = MaxHealthPoints;
        StressLevel = 0;
    }

    void Start()
    {
        MaxHealthPoints = 100;
        HealthPoints = MaxHealthPoints;

        _isSceneLoading = false;
        CalculateHealth();
        HpPanel.HpPanelInstance.Setup(HealthPoints, MaxHealthPoints, StressLevel);
    }

    public void ChangeAmmo(int ammo)
    {
        RevolverAmmo += ammo;
        OnRevolverUsed?.Invoke();
    }

    public void UseRevolver()
    {
        RevolverLoadedAmmo--;
        OnRevolverUsed?.Invoke();
    }

    public int GetReloadValue()
    {
        return  RevolverMaxAmmo - RevolverLoadedAmmo;
    }

    public void ReloadRevolver()
    {
        var reloadValue = GetReloadValue();

        if (reloadValue > 0)
        {
            if (RevolverAmmo < reloadValue)
            {
                RevolverLoadedAmmo += RevolverAmmo;
                RevolverAmmo = 0;
            }
            else
            {
                RevolverLoadedAmmo = RevolverMaxAmmo;
                RevolverAmmo -= reloadValue;
            }

            OnRevolverUsed?.Invoke();
        }
    }

    public void ChangeHealth(long health)
    {
        HealthPoints += health;
        CalculateHealth();

        if (HealthPoints <= 0 && _isSceneLoading == false)
        {
            _isSceneLoading = true;
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    private void CalculateHealth()
    {
        HealthPoints = HealthPoints > MaxHealthPoints ? MaxHealthPoints : HealthPoints;
        HealthPoints = Math.Min(HealthPoints, MaxHealthPoints - StressLevel *10);
        HpPanel.HpPanelInstance.SetHp(HealthPoints, StressLevel);
    }

    public void ChangeStress(int stress)
    {
        StressLevel += stress;
        StressLevel = StressLevel < 0 ? 0 : StressLevel;

        if (StressLevel >= MaxStress && _isSceneLoading == false)
        {
            _isSceneLoading = true;
            ReloadScene();
        }

        CalculateHealth();

        if (HealthPoints <= 0 && _isSceneLoading == false)
        {
            ReloadScene();
        }
    }

    public void ReloadScene()
    {
        _isSceneLoading = true;
        StartCoroutine(LoadYourAsyncScene());
    }

    private IEnumerator LoadYourAsyncScene()
    {
        GameController.GameControllerInstance.AddEvent(GameController.GameEventsEnum.RespawnedForFirstTime);
        LoadingCanvas.LoadingCanvasInstance.gameObject.SetActive(true);

        string currentSceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneName);
        yield return null;
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads

        LoadingCanvas.LoadingCanvasInstance.SetInteraction();
        while (!asyncLoad.isDone)
        {
            yield return null;

            if (asyncLoad.progress >= 0.9f)
            {
                //Wait to you press the space key to activate the Scene
                if (LoadingCanvas.LoadingCanvasInstance.HasInteracted)
                {
                    LoadingCanvas.LoadingCanvasInstance.ResetObject();
                    asyncLoad.allowSceneActivation = true;
                }

                LoadingCanvas.LoadingCanvasInstance.ShowClickText(true);
            }
        }

    }
}
