using UnityEngine;

public class NeuralNetworkCreatureOrgan : NeuralNetworkCreatureVariable, INeuralNetworkCreatureOrgan
{
	public virtual NeuralNetworkCreatureOrganType Type { get; protected set; }

	protected NeuralNetworkCreature _creature;

	protected float _maxVariableValue = 10f;
	protected float _minVariableValue = 0f;

	public NeuralNetworkCreatureOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type)
	{
		_creature = creature;
		Type = type;
	}

	public void Initialize()
	{

	}

	public virtual void Mutate()
	{
		float m = GameController.Instance.RollMutationFactor();
		VariableValue += m;
		VariableValue = Mathf.Clamp(VariableValue, _minVariableValue, _maxVariableValue);
	}

	public virtual NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		throw new System.NotImplementedException();
	}
}