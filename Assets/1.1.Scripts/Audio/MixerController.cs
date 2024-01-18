using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{

    [SerializeField] private AudioMixer myAudioMixer;

    [SerializeField] string volumeparameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    [SerializeField] private Toggle toggle;

    private bool disableToggleEvent;

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeparameter, slider.value);
    }

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    //    slider.onValueChanged.AddListener(handle)
    }

    private void HandleToggleValueChanged(bool enableSound)
    {
        if (disableToggleEvent)
            return;

        if (enableSound)
        {
            slider.value = 0.5f;
        } else
        {
            slider.value = slider.minValue;
        }
    }

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeparameter, slider.value);

    }

    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeparameter, Mathf.Log10(value) * multiplier);
        disableToggleEvent = true;
        toggle.isOn = slider.value > slider.minValue;
        disableToggleEvent = false;

    }

    //public void SetMasterVolume(float sliderValue)
    //{
    //    myAudioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    //}

    //public void SetMusicVolume(float sliderValue)
    //{
    //    myAudioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    //}

}
