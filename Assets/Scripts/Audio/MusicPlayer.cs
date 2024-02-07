using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    public static MusicPlayer MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicPlayer>();
            }
            return instance;
        }
    }

    [Header("List of Tracks")]
    [SerializeField] private Track[] audioTracks;
    private int trackIndex;

    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI trackNameUI;
    [SerializeField] private TextMeshProUGUI trackNumberUI;

    public AudioSource musicPlayerAudioSource;
    public AudioMixerGroup audioMixerGroup;
    public AudioClip titleTrack;
    public bool isGamePlaying;


    // Start is called before the first frame update
    void Start()
    {
        musicPlayerAudioSource.outputAudioMixerGroup = audioMixerGroup;

        musicPlayerAudioSource.clip = audioTracks[trackIndex].trackAudioClip;
        trackNameUI.text = audioTracks[trackIndex].name;
        trackNumberUI.text = "1.";
        PlayTitleMusic();
    }

    public void SkipForwardButton()
    {
        if (trackIndex < audioTracks.Length - 1)
        {
            trackIndex++;
            StartCoroutine(FadeOut(musicPlayerAudioSource, 0.1f));
        }
    }

    public void SkipBackButton()
    {
        if (trackIndex >= 1)
        {
            trackIndex--;
            StartCoroutine(FadeOut(musicPlayerAudioSource, 0.1f));
        }
    }

    public void UpdateTrack(int index)
    {
        musicPlayerAudioSource.clip = audioTracks[index].trackAudioClip;
        trackNameUI.text = audioTracks[index].name;
        trackNumberUI.text = (index + 1).ToString() + ".";
        PlayAudio();
    }

    public void AudioVolume(float volume)
    {
        musicPlayerAudioSource.volume = volume;
    }

    public void PlayAudio()
    {
        StartCoroutine(FadeIn(musicPlayerAudioSource, 0.1f));
    }

    public void PauseAudio()
    {
        musicPlayerAudioSource.Pause();
    }

    public void StopAudio()
    {
        StartCoroutine(FadeOut(musicPlayerAudioSource, 0.1f));
    }

    public void PlayIngameMusic()
    {
        StopTitleMusic();
    }

    public IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        UpdateTrack(trackIndex);
    }

    public IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        audioSource.volume = 0;

        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = startVolume;
    }


    // CHANGE TO TITLE SCREEN
    public void PlayTitleMusic()
    {
        if (isGamePlaying)
        {
            StartCoroutine(FadeInTitleMusic(musicPlayerAudioSource));
        }
        else
        {
            musicPlayerAudioSource.volume = 0.5f;
            musicPlayerAudioSource.clip = titleTrack;
            musicPlayerAudioSource.Play();
        }

    }

    public void StopTitleMusic()
    {
        StartCoroutine(StopTitleMusic(musicPlayerAudioSource));
    }

    public IEnumerator FadeOutToTitleSceen(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 0.3f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;

        StartCoroutine(FadeInTitleMusic(musicPlayerAudioSource));
    }

    public IEnumerator StopTitleMusic(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 0.9f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;

        trackIndex = 0;

        musicPlayerAudioSource.clip = audioTracks[0].trackAudioClip;
        trackNameUI.text = audioTracks[0].name;
        trackNumberUI.text = (0 + 1).ToString() + ".";

        StartCoroutine(FadeIn(musicPlayerAudioSource, 0.6f));
    }



    public IEnumerator FadeInTitleMusic(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        audioSource.volume = 0;
        audioSource.clip = titleTrack;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / 0.6f;
            yield return null;
        }

        audioSource.volume = startVolume;
    }

}

