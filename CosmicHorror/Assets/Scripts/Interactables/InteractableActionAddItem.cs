using UnityEngine;

public class InteractableActionAddItem : InteractableAction
{
    [SerializeField] Sprite pickupSprite;

    public override void PlayAction()
    {
        ItemPanel.ItemPanelInstance.AddItem(pickupSprite);
    }
}
