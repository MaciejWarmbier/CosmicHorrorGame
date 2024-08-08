using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundConfig;

public class MusicPanel : MonoBehaviour
{
    public static MusicPanel MusicPanelInstance;
    public SoundConfig SoundConfig;

    [Header("Main Music")]
    [SerializeField] AudioSource mainMusic;
    [SerializeField] List<AudioEnum> mainMusicList;

    [Header("Random Music")]
    [SerializeField] AudioSource randomMusic;
    [SerializeField] List<AudioEnum> randomMusicList;
    [SerializeField] float randomMusicCooldownMin= 10;
    [SerializeField] float randomMusicCooldownMax = 40;

    private void Awake()
    {
        MusicPanelInstance = this;
    }

    private void OnEnable()
    {
        var mainIndex = UnityEngine.Random.Range(0, mainMusicList.Count);
        mainMusic.clip = SoundConfig.GetAudioClip(mainMusicList[mainIndex]);
        mainMusic.Play();

        StartCoroutine(PlayRandomSounds());
    }

    private IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(randomMusicCooldownMin, randomMusicCooldownMax));

            var mainIndex = UnityEngine.Random.Range(0, randomMusicList.Count);
            randomMusic.clip = SoundConfig.GetAudioClip(randomMusicList[mainIndex]);
            randomMusic.Play();

            if (PlayerStatistics.PlayerStatisticslInstance != null 
                && PlayerStatistics.PlayerStatisticslInstance.HealthPoints <= 0)
            {
                break;
            }
        }
    }
}
