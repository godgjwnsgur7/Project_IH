using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class SettingMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        //ResolutionDropdown = TMP_Dropdown.FindObjectOfType<TMP_Dropdown>();
        

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void OnClickSetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public AudioMixer audioMixer;

    public void OnClickSetMaster(float Volume)
    {
        audioMixer.SetFloat("Master", Volume);
        audioMixer.SetFloat("BGM", Volume);
        audioMixer.SetFloat("Effect", Volume);
    }
    public void OnClickSetBGM(float Volume)
    {
        audioMixer.SetFloat("BGM", Volume);
    }
    public void OnClickSetEffect(float Volume)
    {
        audioMixer.SetFloat("Effect", Volume);
    }

    public void OnClickSetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }

    public void OnClickSetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
