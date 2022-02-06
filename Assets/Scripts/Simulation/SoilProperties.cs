using UnityEngine;

public class SoilProperties
{
	public static float MaxSoilQuality { get { return 10f; } }
	public static float MinSoilQuality { get { return 0f; } }

	public float SoilQuality
	{
		get
		{
			return _soilQuality;
		}
		set
		{
			_soilQuality = Mathf.Clamp(value, MinSoilQuality, MaxSoilQuality);
		}
	}

	private float _soilQuality;

	public void Initialize(float startValue)
	{
		SoilQuality = startValue;
	}

	public float ExtractNutrients(float requestedAmount)
	{
		float output;

		if(SoilQuality > requestedAmount)
		{
			requestedAmount = SoilQuality;
		}

		SoilQuality -= requestedAmount;
		output = requestedAmount;

		return output;
	}
}