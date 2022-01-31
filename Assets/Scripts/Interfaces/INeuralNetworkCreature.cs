
using System.Collections.Generic;

public interface INeuralNetworkCreature
{
	public void Initialize(List<NeuralNetworkCreatureInputOrgan> inputOrgans, List<NeuralNetworkCreatureOutputOrgan> outputOrgans, List<NeuralNetworkCreatureInheritableTrait> traits, int internalLayers, int internalLayerSize);
	public void ProcessNetworkInputs();
	public void ApplyTraits();
	public List<NeuralNetworkCreatureInheritableTrait> GetTraits();
}
