using UnityEngine;

public class BasicRotationOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public BasicRotationOutputOrgan(float speed, NeuralNetworkCreature creature) : base(creature)
	{
		_value = speed;
		_creature = creature;
		Initialize("BasicRotation");
	}

	public override void Process()
	{
		Rotate();
	}

	public void Rotate()
	{
		_creature.transform.Rotate(0, GetOutputValue() * _value, 0, Space.World);
	}
}