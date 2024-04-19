using UnityEngine;

public class InteractableActionPickupItem : InteractableAction
{
    [SerializeField] Sprite pickupSprite;
    [SerializeField] bool deleteAfterInteraction = true;

    public override void PlayAction()
    {
        ItemPanel.ItemPanelInstance.AddItem(pickupSprite);
        
        if(deleteAfterInteraction)
        {
            Destroy(this.gameObject);
        }
    }
}
