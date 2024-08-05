using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    public static LoadingCanvas LoadingCanvasInstance = null;
    public TextMeshProUGUI clickText;
    public List<GameObject> texts = new();

    public bool HasInteracted = false;
    private int usedIndexes;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (LoadingCanvas.LoadingCanvasInstance == null)
        {
            LoadingCanvas.LoadingCanvasInstance = this;
            ResetObject();
        }
        else
        {
            Destroy(this.gameObject);
        }

        usedIndexes = texts.Count;
    }

    public void SetInteraction()
    {
        InputManager.InputManagerInstance.OnInteractionClicked += OnInteractionClicked;
        HasInteracted = false;
    }

    public void ResetObject()
    {
        foreach (GameObject go in texts)
        {
            go.SetActive(false);
        }

        this.gameObject.SetActive(false);
        HasInteracted = false;
        ShowClickText(false);
    }

    private void OnInteractionClicked()
    {
        HasInteracted = true;
    }

    private void OnEnable()
    {
        ShowClickText(false);

        int index = Random.Range(0, usedIndexes);

        texts[index].SetActive(true);
        texts.Add(texts[index]);
        texts.RemoveAt(index);

        usedIndexes--;

        if(usedIndexes <= 0)
        {
            usedIndexes = texts.Count;
        }
    }

    public void ShowClickText(bool show)
    {
        HasInteracted = !show;
        clickText.gameObject.SetActive(show);
    }
}
