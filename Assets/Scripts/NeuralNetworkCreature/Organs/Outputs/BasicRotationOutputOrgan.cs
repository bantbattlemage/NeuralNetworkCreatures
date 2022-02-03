using UnityEngine;

public class BasicRotationOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public BasicRotationOutputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, float speed) : base(creature, type)
	{
		_creature = creature;
		VariableValue = speed;
		Type = type;

		Initialize("BasicRotation");
	}

	public override void Process()
	{
		Rotate();
	}

	public void Rotate()
	{
		_creature.transform.Rotate(0, GetOutputValue() * VariableValue, 0, Space.World);
	}
}