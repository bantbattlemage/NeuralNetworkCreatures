using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverController : MonoBehaviour
{
	public CameraController MainCamera;
	public GameObject PanelGroup;
	public Text PanelTextName;
	public Text PanelText;

	private GameObject _hilightedObject;

	private bool _hold = false;

	private void Update()
	{
		if (!UIFunctions.IsMouseOverGameWindow)
		{
			return;
		}

		if (_hilightedObject != null && PanelGroup.activeInHierarchy)
		{
			UpdateHilightedObjectInfo();
		}

		if(MainCamera.Dragging || Input.GetMouseButtonUp(0))
		{
			return;
		}

		RaycastHit hit;
		Ray ray = MainCamera.GameCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit) && !_hold)
		{
			_hilightedObject = hit.transform.gameObject;
			PanelTextName.text = _hilightedObject.name;

			if(!PanelGroup.activeInHierarchy)
			{
				PanelGroup.SetActive(true);
			}

			if(Input.GetMouseButtonDown(0))
			{
				_hold = true;
			}
		}
		else if(PanelGroup.activeInHierarchy && !_hold)
		{
			PanelGroup.SetActive(false);
		}
		else if(Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
		{
			_hilightedObject = hit.transform.gameObject;
			PanelTextName.text = _hilightedObject.name;
			_hold = true;

			if (!PanelGroup.activeInHierarchy)
			{
				PanelGroup.SetActive(true);
			}
		}
		else if(Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
		{
			if(hit.transform != null)
			{
				_hold = false;

			}
		}
	}

	private void UpdateHilightedObjectInfo()
	{
		if(_hilightedObject.CompareTag("GroundTile"))
		{
			WorldTile tile = _hilightedObject.GetComponent<WorldTile>();
			string text = string.Format("Soil Quality: {0}", tile.GetSoilQuality());
			PanelText.text = text;
		}
		else
		{
			PanelText.text = "";
		}
	}

	public void Close()
	{
		_hold = false;
		PanelGroup.SetActive(false);
	}
}
