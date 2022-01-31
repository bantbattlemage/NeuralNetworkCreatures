using UnityEngine;

public class NeuralNetworkCreatureInheritableTrait : INeuralNeworkCreatureInheritableTrait
{
	public string Name { get; protected set; }
	public float Value { get; protected set; }

	protected NeuralNetworkCreature _creature;

	public NeuralNetworkCreatureInheritableTrait Initialize(NeuralNetworkCreature creature)
	{
		_creature = creature;
		return this;
	}

	public string GetName()
	{
		return Name;
	}

	public string SetName(string name)
	{
		Name = name;
		return Name;
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