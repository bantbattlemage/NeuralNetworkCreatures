using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIFunctions
{
	public static bool IsPointerOverUI
	{
		get
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, results);

			return results.Count > 0;
		}
	}

	public static bool IsMouseOverGameWindow 
	{
		get 
		{ 
			return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); 
		} 
	}
}