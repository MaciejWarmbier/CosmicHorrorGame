using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

public class PlayerStatistics : MonoBehaviour
{
    public static PlayerStatistics PlayerStatisticslInstance = null;

    public long HealthPoints {  get; private set; }
    public long MaxHealthPoints {  get; private set; }
    public long StressLevel {  get; private set; }

    bool _isSceneLoading = false;

    void Start()
    {
        PlayerStatisticslInstance = this;
        MaxHealthPoints = 100;
        HealthPoints = MaxHealthPoints;

        _isSceneLoading = false;
        HpPanel.HpPanelInstance.Setup(HealthPoints, MaxHealthPoints);
    }

    public void ChangeHealth(long health)
    {
        HealthPoints += health;
        HpPanel.HpPanelInstance.SetHp(HealthPoints);

        if (HealthPoints <= 0 && _isSceneLoading == false)
        {
            _isSceneLoading = true;
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    private IEnumerator LoadYourAsyncScene()
    { 
        LoadingCanvas.LoadingCanvasInstance.gameObject.SetActive(true);

        string currentSceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneName);
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
