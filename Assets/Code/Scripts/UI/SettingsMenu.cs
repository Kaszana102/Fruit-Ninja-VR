using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{

	[SerializeField] Slider audioSlider;

	[SerializeField] List<GameObject> fruits = new List<GameObject>();
	[SerializeField] List<TMP_InputField> inputs = new List<TMP_InputField>();



	private TextMeshProUGUI audioValue;


	private void Awake()
	{
		audioValue = gameObject.GetNamedChild("AudioValue").GetComponent<TextMeshProUGUI>();
		SetAllFruits();

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

	private void SetAllFruits()
	{
		foreach (GameObject fruit in fruits) 
		{ 
			TMP_InputField inputValue = fruit.GetComponentInChildren<TMP_InputField>();

			string fruitName = fruit.name.ToString();

			if (!PlayerPrefs.HasKey(fruitName))
			{
				Debug.Log(string.Format("{0} not set, setting it to amount to 5",fruitName));
				PlayerPrefs.SetInt(fruitName, 5);
				PlayerPrefs.Save();
			}
			int fruitAmount = PlayerPrefs.GetInt(fruitName);


			inputValue.text = fruitAmount.ToString();

		}
	}

	public void Set1Fruit(string amount)
	{
		if (!string.IsNullOrEmpty(amount))
		{
			PlayerPrefs.SetInt(fruits[0].name, Int32.Parse(amount));
			PlayerPrefs.Save();
			Debug.Log(string.Format("setting {0} amount to {1}", fruits[0].name, amount));
		}
		else
		{
			PlayerPrefs.SetInt(fruits[0].name, 0);
			PlayerPrefs.Save();
			Debug.Log(string.Format("setting {0} amount to {1}", fruits[0].name, 0));
			fruits[0].GetComponentInChildren<TMP_InputField>().text = "0";
		}
	}

	public void Set2Fruit(string amount)
	{
		if (!string.IsNullOrEmpty(amount))
		{
			PlayerPrefs.SetInt(fruits[1].name, Int32.Parse(amount));
			PlayerPrefs.Save();
			Debug.Log(string.Format("setting {0} amount to {1}", fruits[1].name, amount));
		}
		else
		{
			PlayerPrefs.SetInt(fruits[1].name, 0);
			PlayerPrefs.Save();
			Debug.Log(string.Format("setting {0} amount to {1}", fruits[1].name, 0));
			fruits[1].GetComponentInChildren<TMP_InputField>().text = "0";
		}
	}

	public void Set3Fruit(string amount)
	{
		if (!string.IsNullOrEmpty(amount))
		{
			PlayerPrefs.SetInt(fruits[2].name, Int32.Parse(amount));
			PlayerPrefs.Save();
			Debug.Log(string.Format("setting {0} amount to {1}", fruits[2].name, amount));
		}
		else
		{
			PlayerPrefs.SetInt(fruits[2].name, 0);
			PlayerPrefs.Save();
			Debug.Log(string.Format("setting {0} amount to {1}", fruits[2].name, 0));
			fruits[2].GetComponentInChildren<TMP_InputField>().text = "0";
		}
	}



}
