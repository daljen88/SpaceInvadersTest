using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer SpaceTimeInvadersMixer;

    public const string MASTER_KEY = "masterVolume";
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "SFXVolume";
    public const string LEVEL_WARNS_KEY = "levelWarnsVolume";


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        LoadVolumes();
    }

    private void LoadVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float SFXVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float levelWarnsVolume = PlayerPrefs.GetFloat(LEVEL_WARNS_KEY, 1f);

        SpaceTimeInvadersMixer.SetFloat(VolumeSettingSliders.MIXER_LEVEL_WARNS, Mathf.Log10(levelWarnsVolume) * 20);
        SpaceTimeInvadersMixer.SetFloat(VolumeSettingSliders.MIXER_SFX, Mathf.Log10(SFXVolume) * 20);
        SpaceTimeInvadersMixer.SetFloat(VolumeSettingSliders.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
        SpaceTimeInvadersMixer.SetFloat(VolumeSettingSliders.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
