using UnityEngine;

public class WorldTile : MonoBehaviour
{
	public Vector2Int Location;
	protected SoilProperties Soil;

	private bool _initialized = false;
	private MeshRenderer _meshRendererReference;

	public void Initialize(Vector2Int location, SoilProperties soil = null)
	{
		Location = location;
		gameObject.transform.localPosition = new Vector3(location.x, 0, location.y);
		//gameObject.transform.position = new Vector3(location.x, 0, location.y);
		gameObject.name = location.ToString();

		if(soil == null)
		{
			Soil = new SoilProperties();
			Soil.Initialize(SoilProperties.DefaultSoilQuality);
			SetSoilColor();
		}
		else
		{
			Soil = soil;
		}

		_initialized = true;
	}

	public float ExtractSoilEnergy(float requestedAmount)
	{
		if(requestedAmount < 0)
		{
			throw new System.Exception("negative energy request!");
		}
		else if(requestedAmount == 0 || Soil.SoilQuality == 0)
		{
			return 0;
		}

		if(requestedAmount > Soil.SoilQuality)
		{
			requestedAmount = Soil.SoilQuality;
		}

		Soil.SoilQuality -= requestedAmount;
		SetSoilColor();

		return requestedAmount;
	}

	public void AddSoilEnergy(float amountToGive)
	{
		Soil.SoilQuality += amountToGive;
		SetSoilColor();
	}

	public void SetSoilColor()
	{
		if(_meshRendererReference == null)
		{
			_meshRendererReference = GetComponent<MeshRenderer>();
		}

		int flooredQuality = Mathf.FloorToInt(Soil.SoilQuality - 1);
		if(flooredQuality < 0)
		{
			flooredQuality = 0;
		}

		_meshRendererReference.material.SetColor("_Color", SoilColors.Instance.Colors[flooredQuality]);
	}

	private void OnDestroy()
	{
		Destroy(GetComponent<MeshRenderer>().material);
	}
}