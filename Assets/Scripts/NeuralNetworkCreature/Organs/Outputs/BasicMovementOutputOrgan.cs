using UnityEngine;

public class BasicMovementOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public float Speed;
	
	public BasicMovementOutputOrgan(float speed, NeuralNetworkCreature creature) : base(creature)
	{
		Speed = speed;
		_creature = creature;
		Initialize("BasicMovement");
	}

	public override void Process()
	{
		MoveForward();
	}

	public void MoveForward()
	{
		Vector3 newPosition = _creature.transform.position + (_creature.transform.forward * Speed * GetOutputValue());

		if(!GameController.Instance.IsOutOfBounds(newPosition))
		{
			_creature.transform.position = newPosition;
		}
	}
}