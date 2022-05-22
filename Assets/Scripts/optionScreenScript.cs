using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class optionScreenScript : MonoBehaviour
{

    public Toggle fullScreenToggle, vsyncToggle;
    private int selectedResolution;

    public List<resolutionItem> resolutions = new List<resolutionItem>();

    public TMP_Text resolutionLabel;

    public AudioMixer Mixer;
    public TMP_Text masterNumber, musicNumber, sfxNumber;
    public Slider masterSlider, musicSlider, sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        //in functie de ce setari active are jocul updatam screenul de setari
        fullScreenToggle.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        //temp code
        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if( Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                selectedResolution = i;
                UpdateResolutionTitle();
            }
        }

        if(foundRes == false)
        {
            resolutionItem newResolution = new resolutionItem();
            newResolution.horizontal = Screen.width;
            newResolution.vertical = Screen.height;

            resolutions.Add(newResolution);
            selectedResolution = resolutions.Count - 1;

            UpdateResolutionTitle();
        }

        //nu e nevoie de volume mereu
        float volume = 0f;
        Mixer.GetFloat("MasterVolume", out volume);
        masterSlider.value = volume;

        Mixer.GetFloat("MusicVolume", out volume);
        musicSlider.value = volume;

        Mixer.GetFloat("SFXVolume", out volume);
        sfxSlider.value = volume;


        masterNumber.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicNumber.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxNumber.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RezLeftButton()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResolutionTitle();
    }

    public void RezRight()
    {
        selectedResolution++;
        // > 2
        if(selectedResolution > resolutions.Count -1 )
        {
            // = 2
            selectedResolution = resolutions.Count -1;
        }

        UpdateResolutionTitle();
    }

    public void UpdateResolutionTitle()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        //Screen.fullScreen = fullScreenToggle.isOn;

        if(vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullScreenToggle.isOn);

    }

    public void SetMasterVolume()
    {
        masterNumber.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();

        Mixer.SetFloat("MasterVolume", masterSlider.value);

        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        musicNumber.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        Mixer.SetFloat("MusicVolume", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        sfxNumber.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

        Mixer.SetFloat("SFXVolume", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}

[System.Serializable]
public class resolutionItem
{
    public int horizontal, vertical;
}
