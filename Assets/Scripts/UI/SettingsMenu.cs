using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
	// SettingsMenu handles the in-game configurable settings
	public class SettingsMenu : MonoBehaviour
	{
		public  AudioMixer   audioMixer;
		public  Dropdown     resDropdown;
		public  Dropdown     graphicsDropdown;
		private Resolution[] _resolutions;

		private void Start()
		{
			// Get the resolutions available for project
			_resolutions = Screen.resolutions;
			// Clear the dropdown menu
			resDropdown.ClearOptions();

			int currentResIndex = 0;
			int graphicsIndex = QualitySettings.GetQualityLevel();

			// Convert the resolutions to a list
			List<string> resOptions = new List<string>();
			for (int i = 0; i < _resolutions.Length; i++)
			{
				string resOption = _resolutions[i].width + " x " + _resolutions[i].height;
				resOptions.Add(resOption);

				//check current resolution
				if (_resolutions[i].width == Screen.currentResolution.width &&
				    _resolutions[i].height == Screen.currentResolution.height)
					currentResIndex = i;
			}

			// Add resOptions to dropdown
			resDropdown.AddOptions(resOptions);

			// Set dropdown to current resolution and current graphics settings
			resDropdown.value = currentResIndex;
			resDropdown.RefreshShownValue();

			graphicsDropdown.value = graphicsIndex;
			graphicsDropdown.RefreshShownValue();
		}

		public void SetResolution(int resIndex)
		{
			Resolution resolution = _resolutions[resIndex];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}

		// Changes the volume of the mixer
		public void SetVolume(float volume)
		{
			audioMixer.SetFloat("Volume", volume);
		}

		// Sets quality settings (found in Project Settings)
		public void SetQuality(int quality)
		{
			QualitySettings.SetQualityLevel(quality);
		}

		// Toggles fullscreen
		public void SetFullscreen(bool isFullscreen)
		{
			Screen.fullScreen = isFullscreen;
		}
	}
}