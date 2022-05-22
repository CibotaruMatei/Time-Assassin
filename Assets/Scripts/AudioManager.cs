using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{

    public AudioMixer Mixer;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            Mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        }

        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            Mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        }

        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            Mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }
    }

    // Update is called once per frame
  
}
