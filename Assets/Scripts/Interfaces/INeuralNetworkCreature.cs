
using System.Collections.Generic;
using UnityEngine;

public interface INeuralNetworkCreature
{
	public void Initialize(List<NeuralNetworkCreatureInputOrgan> inputOrgans, List<NeuralNetworkCreatureOutputOrgan> outputOrgans, List<NeuralNetworkCreatureInheritableTrait> traits, int internalLayers, int internalLayerSize);
	public void ProcessNetworkInputs();

	public void ApplyTraits();
	public List<NeuralNetworkCreatureInheritableTrait> GetTraits();
	public NeuralNetworkCreature Reproduce(NeuralNetworkCreature otherParent, Vector3 location);
	public void Mutate(int mutationChance, float mutationStrength);
}