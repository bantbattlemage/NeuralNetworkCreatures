using UnityEngine;

public class NeuralNetworkCreatureInheritableTrait : NeuralNetworkCreatureOrgan
{
	public float Value { get; protected set; }

	public NeuralNetworkCreatureInheritableTrait(NeuralNetworkCreature creature) : base(creature)
	{
		_creature = creature;
	}

	public virtual NeuralNetworkCreatureInheritableTrait CreateDeepCopy(NeuralNetworkCreature creature)
	{
		NeuralNetworkCreatureInheritableTrait newTrait = new NeuralNetworkCreatureInheritableTrait(creature);
		return newTrait;
	}

	public float GetValue()
	{
		return Value;
	}

	public float SetValue(float value)
	{
		Value = value;
		return Value;
	}

	public virtual void ApplyTraitValue()
	{

	}

	public virtual void MutateTraitValue()
	{

	}
}