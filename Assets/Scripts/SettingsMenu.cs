using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resDropdown;
    private Resolution[] _resolutions;

    private void Start()
    {
        //get resolutions available for project
        _resolutions = Screen.resolutions;

        //clear dropdown
        resDropdown.ClearOptions();

        //convert resolutions to list
        int currentResIndex = 0;
        List<string> resOptions = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string resOption = _resolutions[i].width + " x " + _resolutions[i].height;
            resOptions.Add(resOption);

            //check current resolution
            if(_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        //add resOptions to dropdown
        resDropdown.AddOptions(resOptions);
        //set dropdown to current resolution
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = _resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //changes volume of Mainmixer
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    //Sets quality settings (found in Project Settings)
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    //fullscreen toggle
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
