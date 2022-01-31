using UnityEngine;

[System.Serializable]
public class NeuralNetworkCreatureOrgan : INeuralNetworkCreatureOrgan
{
	public string Name { get; protected set; }

	public string GetName()
	{
		return Name;
	}

	public void SetName(string name)
	{
		Name = name;
	}
}