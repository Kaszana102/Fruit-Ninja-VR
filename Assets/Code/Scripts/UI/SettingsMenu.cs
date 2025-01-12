using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
	public Slider audioSlider;
	private TextMeshProUGUI audioValue;


	private void Awake()
	{
		audioValue = gameObject.GetNamedChild("AudioValue").GetComponent<TextMeshProUGUI>();

		if (!PlayerPrefs.HasKey("Audio"))
		{
			Debug.Log("Audio not set, setting it to 0.5");
			PlayerPrefs.SetFloat("Audio", 0.5f);
			PlayerPrefs.Save();
		}


		audioValue.text = PlayerPrefs.GetFloat("Audio").ToString("0.00").Replace(',', '.');
		audioSlider.value = PlayerPrefs.GetFloat("Audio");

	}

	private void Start()
    {
        
    }

	public void SetAudio(float volume)
	{
		float roundedVolume = (float)Math.Round(volume, 2);

		PlayerPrefs.SetFloat("Audio", roundedVolume);
		PlayerPrefs.Save();
		audioValue.text = roundedVolume.ToString("0.00").Replace(',', '.');
		// Debug.Log("Changed audio volume to " + roundedVolume);
	}


}
