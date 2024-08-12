using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LetterCanvas : MonoBehaviour
{
    public static LetterCanvas LetterCanvasInstance = null;
    [SerializeField] GameObject firstPage;
    [SerializeField] GameObject secondPage;
    public Canvas canvas;

    public bool HasInteracted = false;
    private int usedIndexes;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (LetterCanvas.LetterCanvasInstance == null)
        {
            LetterCanvas.LetterCanvasInstance = this;
            Show();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Show()
    {
        Cursor.visible = false;
        firstPage.SetActive(true);
        secondPage.SetActive(false);
        canvas.enabled = !canvas.enabled;
        Time.timeScale = canvas.enabled ? 0 : 1;
    }

    public void Change()
    {
        if (canvas.enabled)
        {
            Cursor.visible = false;
            firstPage.SetActive(false);
            secondPage.SetActive(true);
        }
    }
}
