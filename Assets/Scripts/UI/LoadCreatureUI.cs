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
		_cachedFilePaths = new List<string>();
		UIGroup.SetActive(false);
	}

	public void OnSaveButtonPressed()
	{
		GameController.Instance.SaveBestCreature();
		Debug.Log("Saved");
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

	public void Delete()
	{
		if (_cachedFilePaths.Count == 0 || OptionsDropdown.value == 0)
		{
			OnCloseButtonPressed();
			return;
		}

		string path = _cachedFilePaths[OptionsDropdown.value - 1];
		File.Delete(path);
		Initialize();
	}

	public void Initialize()
	{
		_cachedFilePaths = new List<string>();
		OptionsDropdown.options = new List<Dropdown.OptionData>();
		OptionsDropdown.options.Add(new Dropdown.OptionData("None"));
		OptionsDropdown.value = 0;

		DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
		FileInfo[] fileInfo = info.GetFiles();
		foreach(FileInfo file in fileInfo)
		{
			string name = file.Name;
			string filePath = Application.persistentDataPath + "/" + name;
			_cachedFilePaths.Add(filePath);
			OptionsDropdown.options.Add(new Dropdown.OptionData(name));
		}
	}
}