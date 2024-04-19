using UnityEngine;

public class InteractableActionChangeSprite : InteractableAction
{
    [SerializeField] Sprite changeSprite;
    [SerializeField] SpriteRenderer spriteRenderer;

    public override void PlayAction()
    {
        spriteRenderer.sprite = changeSprite;
    }
}
