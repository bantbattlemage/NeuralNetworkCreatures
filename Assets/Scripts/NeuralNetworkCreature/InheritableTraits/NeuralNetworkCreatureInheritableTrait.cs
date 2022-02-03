using UnityEngine;

public class NeuralNetworkCreatureInheritableTrait : NeuralNetworkCreatureOrgan, INeuralNeworkCreatureInheritableTrait
{
	public NeuralNetworkCreatureInheritableTrait(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type) : base(creature, type)
	{

	}

	public virtual NeuralNetworkCreatureInheritableTrait CreateDeepCopy(NeuralNetworkCreature creature)
	{
		NeuralNetworkCreatureInheritableTrait newTrait = new NeuralNetworkCreatureInheritableTrait(creature, Type);
		return newTrait;
	}

	public virtual void ApplyTraitValue()
	{
		throw new System.NotImplementedException();
	}
}