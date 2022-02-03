using UnityEngine;

public class BasicMovementOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public BasicMovementOutputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, float speed) : base(creature, type)
	{
		_creature = creature;
		VariableValue = speed;
		Type = type;

		Initialize("BasicMovement");
	}

	public override void Process()
	{
		MoveForward();
	}

	public void MoveForward()
	{
		Vector3 newPosition = _creature.transform.position + (_creature.transform.forward * VariableValue * GetOutputValue());

		if(!GameController.Instance.IsOutOfBounds(newPosition))
		{
			_creature.transform.position = newPosition;
		}
	}
}