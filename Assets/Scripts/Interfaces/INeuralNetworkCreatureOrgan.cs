using System.Collections.Generic;

public interface INeuralNetworkCreatureOrgan
{
	public void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null);
	public void Mutate();
	public NeuralNetworkCreatureOrgan CreateDeepCopy();
}