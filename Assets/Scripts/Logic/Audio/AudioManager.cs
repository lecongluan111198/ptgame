using Assets.Scripts.Logic.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource myAudioSource;
    public AudioMixer theMixer;

    public Slider musicSlider, effectSlider;
    public Text musicLabel, effectLabel;
    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        loadSaveVol();
    }

    public void playSound(SoundEnum sound)
    {
        if (audioClips != null)
        {
            AudioClip clip = audioClips[(int)sound];
            if (myAudioSource != null && clip != null)
            {
                myAudioSource.PlayOneShot(clip);
            }
        }
    }

    public void SetMusicVol()
    {
        musicLabel.text = ((int)(musicSlider.value * 100)).ToString();
        int musicVolume = (int)(musicSlider.value * 100 - 80);
        theMixer.SetFloat("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVol()
    {

        effectLabel.text = ((int)(effectSlider.value * 100)).ToString();
        int musicVolume = (int)(effectSlider.value * 100 - 80);
        theMixer.SetFloat("SFXVol", musicVolume);
        PlayerPrefs.SetFloat("SFXVol", effectSlider.value);

    }

    private void loadSaveVol()
    {
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            theMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
            effectSlider.value = PlayerPrefs.GetFloat("SFXVol");
        }
        musicLabel.text = ((int)(musicSlider.value * 100)).ToString();
        effectLabel.text = ((int)(effectSlider.value * 100)).ToString();
    }
}
