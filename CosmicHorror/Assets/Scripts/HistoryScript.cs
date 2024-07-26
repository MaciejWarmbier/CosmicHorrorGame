using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryScript : InteractableAction
{
    [SerializeField] float animationTime = 1f;

    [SerializeField] List<Sprite> flintSprites = new List<Sprite>();
    [SerializeField] List<Sprite> woodSprites = new List<Sprite>();
    [SerializeField] SpriteRenderer flintImage;
    [SerializeField] SpriteRenderer woodImage;
    [SerializeField] SpriteRenderer sparkImage;

    private bool _isReset = false;

    private void Start()
    {
        if (GameController.GameControllerInstance.isFlintHistoryVisited)
        {
            StartCoroutine(PlayEndAnimation());
        }
    }

    public override void PlayAction()
    {
        _isReset = false;
        StartCoroutine(PlayBegginingAnimation());
    }

    private IEnumerator PlayBegginingAnimation()
    {
        GameController.GameControllerInstance.isFlintHistoryVisited = true;

        flintImage.sprite = flintSprites[0];
        yield return new WaitForSeconds(animationTime);
        flintImage.sprite = flintSprites[1];
        yield return new WaitForSeconds(animationTime); 
        flintImage.sprite = flintSprites[2];
        sparkImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(animationTime);
        sparkImage.gameObject.SetActive(false);
        flintImage.sprite = flintSprites[0];
        yield return new WaitForSeconds(animationTime);
        flintImage.sprite = flintSprites[1];
        yield return new WaitForSeconds(animationTime); 
        flintImage.sprite = flintSprites[2];
        sparkImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(animationTime);
        sparkImage.gameObject.SetActive(false);

        StartCoroutine(PlayEndAnimation());
    }

    private IEnumerator PlayEndAnimation()
    {
        while (!_isReset)
        {
            woodImage.sprite = woodSprites[1];
            yield return new WaitForSeconds(animationTime);
            woodImage.sprite = woodSprites[2];
            yield return new WaitForSeconds(animationTime);
        }
    }

    public override void ResetAction()
    {
        _isReset = true;
    }
}
