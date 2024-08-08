using UnityEngine;

public class InteractableActionPickupItem : InteractableAction
{
    [SerializeField] bool deleteAfterInteraction = true;
    [SerializeField] AudioSource audioSource;

    public override void PlayAction()
    {
        audioSource.clip = MusicPanel.MusicPanelInstance.SoundConfig.GetAudioClip(SoundConfig.AudioEnum.pickup);
        audioSource.Play();

        if (deleteAfterInteraction)
        {
            Destroy(this.gameObject);
        }
    }
}
