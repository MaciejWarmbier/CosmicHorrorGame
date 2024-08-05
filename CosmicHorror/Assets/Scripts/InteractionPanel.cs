using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPanel : MonoBehaviour
{
    public static InteractionPanel InteractionPanelInstance;

    [SerializeField] GameObject interactionText;

    private void Awake()
    {
        InteractionPanelInstance = this;
    }

    public void SetInteractionTextActive(bool active)
    {
        interactionText.SetActive(active);
    }
}
