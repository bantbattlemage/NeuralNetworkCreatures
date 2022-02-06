using UnityEngine;

public class WorldTile : MonoBehaviour
{
	public Vector2Int Location;
	public SoilProperties Soil;

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
		}
		else
		{
			Soil = soil;
		}

		_initialized = true;
	}

	public void SetSoilColor()
	{
		if(_meshRendererReference == null)
		{
			_meshRendererReference = GetComponent<MeshRenderer>();
		}

		Color c = new Color();

		//switch (Soil.)
		//{
		//	default:
		//		break;
		//}
	}

	private void OnDestroy()
	{
		Destroy(GetComponent<MeshRenderer>().material);
	}
}