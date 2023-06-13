using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettingSliders : MonoBehaviour
{
    public AudioMixer SpaceTimeInvadersMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider SFXSlider;
    public Slider levelWarnsSlider;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_LEVEL_WARNS = "LevelWarnsVolume";

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
        levelWarnsSlider.onValueChanged.AddListener(SetLevelWarnsVolume);
        
    }
    
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);   
        masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);   
        SFXSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);   
        levelWarnsSlider.value = PlayerPrefs.GetFloat(AudioManager.LEVEL_WARNS_KEY, 1f);
    }

    void OnDisable()
    {
        SavePrefsToAudioManager();
    }

    private void SetLevelWarnsVolume(float value)
    {
        SpaceTimeInvadersMixer.SetFloat(MIXER_LEVEL_WARNS, Mathf.Log10(value) * 20);
    }

    private void SetSFXVolume(float value)
    {
        SpaceTimeInvadersMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }

    private void SetMusicVolume(float value)
    {
        SpaceTimeInvadersMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    private void SetMasterVolume(float value)
    {
        SpaceTimeInvadersMixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
    }
    public void SavePrefsToAudioManager()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.LEVEL_WARNS_KEY, levelWarnsSlider.value);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
