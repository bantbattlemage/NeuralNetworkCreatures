using System.Linq;
using UnityEngine;

public class NeuralNetworkCreatureInheritableTrait : NeuralNetworkCreatureOrgan, INeuralNeworkCreatureInheritableTrait
{
	public virtual NeuralNetworkCreatureInheritableTrait CreateDeepCopy(NeuralNetworkCreature creature)
	{
		NeuralNetworkCreatureInheritableTrait newTrait = new NeuralNetworkCreatureInheritableTrait();
		newTrait.Initialize(creature, Type, OrganVariables.Values.ToList());

		return newTrait;
	}

	public virtual void ApplyTraitValue()
	{
		throw new System.NotImplementedException();
	}
}