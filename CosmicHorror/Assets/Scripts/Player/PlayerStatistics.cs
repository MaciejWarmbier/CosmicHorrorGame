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

    void Start()
    {
        PlayerStatisticslInstance = this;
        MaxHealthPoints = 100;
        HealthPoints = MaxHealthPoints;

        HpPanel.HpPanelInstance.Setup(HealthPoints, MaxHealthPoints);
    }

    public void ChangeHealth(long health)
    {
        HealthPoints += health;
        HpPanel.HpPanelInstance.SetHp(HealthPoints);

        if (HealthPoints <= 0)
        {
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    private IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        LoadingCanvas.LoadingCanvasInstance.gameObject.SetActive(true);

        string currentSceneName = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;

            if (asyncLoad.progress >= 0.9f)
            {
                LoadingCanvas.LoadingCanvasInstance.gameObject.SetActive(false);
                asyncLoad.allowSceneActivation = true;
            }
        }
    }
}
