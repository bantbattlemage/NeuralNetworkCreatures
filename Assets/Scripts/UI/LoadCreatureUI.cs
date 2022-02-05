using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadCreatureUI : MonoBehaviour
{
	public GameObject UIGroup;
	public Button OpenDialogueButton;
	public Button CloseDialogueButton;
	public Dropdown OptionsDropdown;

	private List<string> _cachedFilePaths = new List<string>();

	private void Start()
	{
		UIGroup.SetActive(false);
	}

	public void OnOpenButtonPressed()
	{
		UIGroup.SetActive(true);
		Initialize();
	}

	public void OnCloseButtonPressed()
	{
		UIGroup.SetActive(false);
		_cachedFilePaths = new List<string>();
	}

	public void OnValueChanged()
	{

	}

	public void Load()
	{
		if(_cachedFilePaths.Count == 0 || OptionsDropdown.value == 0)
		{
			OnCloseButtonPressed();
			return;
		}

		GameController.Instance.LoadCreature(_cachedFilePaths[OptionsDropdown.value - 1]);
		OnCloseButtonPressed();
	}

	public void Initialize()
	{
		_cachedFilePaths = new List<string>();
		OptionsDropdown.options = new List<Dropdown.OptionData>();
		OptionsDropdown.options.Add(new Dropdown.OptionData("None"));
		OptionsDropdown.value = 0;

		string fileName = "newSavedCreature.json";
		string filePath = Application.persistentDataPath + "/" + fileName;

		if (File.Exists(filePath))
		{
			int count = 0;
			while (File.Exists(filePath))
			{
				_cachedFilePaths.Add(filePath);
				OptionsDropdown.options.Add(new Dropdown.OptionData(fileName));
				fileName = string.Format("/newSavedCreature{0}.json", count);
				filePath = Application.persistentDataPath + "/" + fileName;
				count++;
			}
		}
	}
}