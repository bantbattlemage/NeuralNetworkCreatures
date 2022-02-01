using UnityEngine;

public class NeuralNetworkCreatureOrgan : NeuralNetworkCreatureVariable, INeuralNetworkCreatureOrgan
{
	protected NeuralNetworkCreature _creature;

	protected float _maxVariableValue = 10f;
	protected float _minVariableValue = 0f;

	public NeuralNetworkCreatureOrgan(NeuralNetworkCreature creature)
	{
		_creature = creature;
	}

	public virtual void Mutate()
	{
		float m = GameController.Instance.RollMutationFactor();
		_value += m;
		_value = Mathf.Clamp(_value, _minVariableValue, _maxVariableValue);
	}
}