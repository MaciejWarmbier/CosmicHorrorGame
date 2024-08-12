using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryScriptPlant : InteractableAction
{
    [SerializeField] float animationTime = 1f;

    [SerializeField] List<Sprite> goundSprites = new List<Sprite>();
    [SerializeField] List<SpriteRenderer> toolImages;
    [SerializeField] List<SpriteRenderer> rainImages;
    [SerializeField] SpriteRenderer groundImage;

    private bool _isReset = false;

    private void Start()
    {
        if (GameController.GameControllerInstance.WasEventDone(GameController.GameEventsEnum.PlantHistory))
        {
            toolImages[2].gameObject.SetActive(true);

            toolImages[0].gameObject.SetActive(false);
            toolImages[1].gameObject.SetActive(false);
            rainImages[0].gameObject.SetActive(false);
            rainImages[1].gameObject.SetActive(false);
            StartCoroutine(PlayEndAnimation());
        }
    }

    public override void PlayAction()
    {
        if (!GameController.GameControllerInstance.WasEventDone(GameController.GameEventsEnum.PlantHistory))
        {
            _isReset = false;
            StartCoroutine(PlayBegginingAnimation());
        }
    }

    private IEnumerator PlayBegginingAnimation()
    {
        GameController.GameControllerInstance.AddEvent(GameController.GameEventsEnum.PlantHistory);

        groundImage.sprite = goundSprites[0];
        toolImages[0].gameObject.SetActive(true);
        toolImages[2].gameObject.SetActive(false);
        toolImages[1].gameObject.SetActive(false);
        rainImages[0].gameObject.SetActive(false);
        rainImages[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(animationTime);
        toolImages[0].gameObject.SetActive(false);
        toolImages[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(animationTime);
        toolImages[1].gameObject.SetActive(false);
        toolImages[2].gameObject.SetActive(true);
        groundImage.sprite = goundSprites[1];
        yield return new WaitForSeconds(animationTime);
        rainImages[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(animationTime);
        rainImages[0].gameObject.SetActive(false);
        rainImages[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(animationTime);
        rainImages[1].gameObject.SetActive(false);
        groundImage.sprite = goundSprites[2];

        StartCoroutine(PlayEndAnimation());
    }

    private IEnumerator PlayEndAnimation()
    {
        while (!_isReset)
        {
            groundImage.sprite = goundSprites[3];
            yield return new WaitForSeconds(animationTime);
            groundImage.sprite = goundSprites[2];
            yield return new WaitForSeconds(animationTime);
        }
    }

    public override void ResetAction()
    {
        _isReset = true;
    }
}
