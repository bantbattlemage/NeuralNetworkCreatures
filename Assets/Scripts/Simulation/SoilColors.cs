using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilColors : MonoBehaviour
{
	public Color[] Colors;

	public static SoilColors Instance;

	private void Start()
	{
		Instance = this;
	}

	public Color GetSoilColor(int soilQuality)
	{
		return Colors[soilQuality];
	}
}