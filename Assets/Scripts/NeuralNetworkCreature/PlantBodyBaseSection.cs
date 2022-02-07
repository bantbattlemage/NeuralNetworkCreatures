using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBodyBaseSection : MonoBehaviour
{
	public GameObject BasePrefabReference;
	public GameObject BaseLeafReference;
	public Transform[] AttachmentTransforms;

	public GameObject Model;
	public float Height;
	public int NumberOfLeaves;

	public Dictionary<string, Transform> AttachmentPoints = new Dictionary<string, Transform>();
	public Dictionary<string, GameObject> Attachments = new Dictionary<string, GameObject>();
	public PlantBodyBaseSection SubSection = null;

	public static float BaseSectionMinSize { get { return 0.1f; } }
	public static float BaseSectionMaxSize { get { return 1f; } }

	private void Awake()
	{
		AttachmentPoints.Add("Top", AttachmentTransforms[0]);
		AttachmentPoints.Add("Bottom", AttachmentTransforms[1]);
		AttachmentPoints.Add("Front", AttachmentTransforms[2]);
		AttachmentPoints.Add("Back", AttachmentTransforms[3]);
		AttachmentPoints.Add("Left", AttachmentTransforms[4]);
		AttachmentPoints.Add("Right", AttachmentTransforms[5]);
	}

	private void Start()
	{
		Height = BaseSectionMinSize;
		transform.localScale = new Vector3(transform.localScale.x, Height, transform.localScale.z);
	}

	public void InitializeLeaves(int numberOfLeaves)
	{
		for (int i = 0; i < numberOfLeaves; i++)
		{
			AddLeaf();
		}
	}

	public void AddLeaf()
	{
		if(Attachments.Count >= 4)
		{
			throw new System.Exception("max attachments already!");
		}

		List<string> unusedPoints = new List<string> { "Front", "Back", "Left", "Right" };
		foreach(string key in Attachments.Keys)
		{
			unusedPoints.Remove(key);
		}

		string pointToUse = unusedPoints[Random.Range(0, unusedPoints.Count)];

		GameObject newLeaf = Instantiate(BaseLeafReference, AttachmentPoints[pointToUse]);
		newLeaf.transform.localPosition = new Vector3(0, newLeaf.transform.localScale.y, 0);
		newLeaf.GetComponent<MeshRenderer>().material.SetColor("_Color", Model.GetComponent<MeshRenderer>().material.color);

		Attachments.Add(pointToUse, newLeaf);
	}

	public float GetTotalHeight()
	{
		float sum = Height;

		if(SubSection != null)
		{
			sum += SubSection.GetTotalHeight();
		}

		return sum;
	}

	public void Grow(float growthValue)
	{
		if(SubSection != null)
		{
			SubSection.Grow(growthValue);
			return;
		}

		float newHeight = transform.localScale.y + growthValue;

		if(newHeight > BaseSectionMaxSize)
		{
			Height = BaseSectionMaxSize;
			PlantBodyBaseSection newSection = Instantiate(BasePrefabReference, AttachmentPoints["Top"]).GetComponent<PlantBodyBaseSection>();
			newSection.transform.localScale = new Vector3(newSection.transform.localScale.x, BaseSectionMinSize, newSection.transform.localScale.z);
			newSection.transform.localPosition = new Vector3(0, BaseSectionMinSize, 0);
			newSection.InitializeLeaves(NumberOfLeaves);
			SubSection = newSection;
		}
		else
		{
			Height = Mathf.Clamp(transform.localScale.y + growthValue, BaseSectionMinSize, BaseSectionMaxSize);
		}

		transform.localScale = new Vector3(transform.localScale.x, Height, transform.localScale.z);
		transform.localPosition = new Vector3(transform.localPosition.x, Height, transform.localPosition.z);
	}
}
