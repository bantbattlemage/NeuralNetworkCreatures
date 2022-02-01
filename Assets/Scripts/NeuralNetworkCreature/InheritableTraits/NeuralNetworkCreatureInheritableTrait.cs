using UnityEngine;

public class NeuralNetworkCreatureInheritableTrait : NeuralNetworkCreatureOrgan
{
	public NeuralNetworkCreatureInheritableTrait(NeuralNetworkCreature creature) : base(creature)
	{
		_creature = creature;
	}

	public virtual NeuralNetworkCreatureInheritableTrait CreateDeepCopy(NeuralNetworkCreature creature)
	{
		NeuralNetworkCreatureInheritableTrait newTrait = new NeuralNetworkCreatureInheritableTrait(creature);
		return newTrait;
	}

	public virtual void ApplyTraitValue()
	{

	}
}