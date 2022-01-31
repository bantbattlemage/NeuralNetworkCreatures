using UnityEngine;

public class BasicRotationOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public float Speed;
	private NeuralNetworkCreature _creature;

	public BasicRotationOutputOrgan(float speed, NeuralNetworkCreature creature)
	{
		Speed = speed;
		_creature = creature;
		Initialize("BasicRotation");
	}

	public override void Process()
	{
		Rotate();
	}

	public void Rotate()
	{
		_creature.transform.Rotate(0, GetOutputValue() * Speed, 0, Space.World);
	}
}