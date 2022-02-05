using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetworkCreatureData
{
	public Dictionary<string, NeuralNetworkCreatureInputOrgan> InputOrgans = new Dictionary<string, NeuralNetworkCreatureInputOrgan>();
	public Dictionary<string, NeuralNetworkCreatureOutputOrgan> OutputOrgans = new Dictionary<string, NeuralNetworkCreatureOutputOrgan>();
	public Dictionary<string, NeuralNetworkCreatureInheritableTrait> Traits = new Dictionary<string, NeuralNetworkCreatureInheritableTrait>();
	public NeuralNetwork Network;
}