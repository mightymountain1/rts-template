using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }
    public Sound[] sounds;


    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.playOnAwake = s.playOnAwake;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
        }

    }


    // Update is called once per frame
    public void Play(string name, float volume)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
        s.source.volume = volume;

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
  
    }


    public void PlayRandomPitch(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.pitch = Random.Range(1f - 0.02f, 1f + 0.02f);

        s.source.Play();
    }


    public void PlayRandomPitchSet(string name, float range)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.pitch = Random.Range(1f - range, 1f + range);

        s.source.Play();
    }


    // Play One Shot passing the volume and delay as a parameter
    public void PlayOneShot(string clipName, float volume, float delay)
    {
        StartCoroutine(PlaySoundCR(clipName, volume, delay));
    }

    IEnumerator PlaySoundCR(string clipName, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayOneShotGO(clipName, volume);
    }


    public void PlayOneShotGO(string name, float volume)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.PlayOneShot(s.clip, volume);
    }


    // Play Clip at specific location. Good for spatial sound
    public void PlayClipAtPoint(string clipName, float volume, float delay, Vector3 location)
    {
        StartCoroutine(PlayClipAtPointCR(clipName, location, volume, delay));
    }


    IEnumerator PlayClipAtPointCR(string clipName, Vector3 location, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayClipAtPointGO(clipName, location, volume);
    }


    public void PlayClipAtPointGO(string name, Vector3 location, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        AudioSource.PlayClipAtPoint(s.clip, location, volume);

    }

}