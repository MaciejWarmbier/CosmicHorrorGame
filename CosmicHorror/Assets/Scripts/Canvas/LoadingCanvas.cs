using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
    public static LoadingCanvas LoadingCanvasInstance = null;
    public TextMeshProUGUI loadingText;

    int loadingTime = 1;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (LoadingCanvas.LoadingCanvasInstance == null)
        {
            LoadingCanvas.LoadingCanvasInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        loadingText.text = $"Loadin {loadingTime}...";
        loadingTime++;
    }
}
