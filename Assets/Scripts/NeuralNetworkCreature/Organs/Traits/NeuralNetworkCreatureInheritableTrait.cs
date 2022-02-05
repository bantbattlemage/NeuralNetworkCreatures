using System.Linq;
using UnityEngine;

public class NeuralNetworkCreatureInheritableTrait : NeuralNetworkCreatureOrgan, INeuralNeworkCreatureInheritableTrait
{
	public override NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		NeuralNetworkCreatureInheritableTrait copy = new NeuralNetworkCreatureInheritableTrait();
		copy.Initialize(_creature, Type, OrganVariables.Values.ToList());
		copy.MutatableVariable = MutatableVariable.Copy();

		return copy;
	}

	public virtual NeuralNetworkCreatureOrgan CreateDeepCopy(NeuralNetworkCreature creature)
	{
		NeuralNetworkCreatureInheritableTrait copy = new NeuralNetworkCreatureInheritableTrait();
		copy.Initialize(creature, Type, OrganVariables.Values.ToList());
		copy.MutatableVariable = MutatableVariable.Copy();

		return copy;
	}

	public virtual void ApplyTraitValue()
	{
		//throw new System.NotImplementedException();
	}
}