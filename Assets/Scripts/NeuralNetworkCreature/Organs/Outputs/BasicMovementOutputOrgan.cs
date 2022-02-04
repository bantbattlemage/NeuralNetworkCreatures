using System.Collections.Generic;
using UnityEngine;

public class BasicMovementOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public override void Process()
	{
		MoveForward();
	}

	private void MoveForward()
	{
		Vector3 newPosition = _creature.transform.position + (_creature.transform.forward * MutatableVariable.Value * GetOutputValue());

		if(!GameController.Instance.IsOutOfBounds(newPosition))
		{
			_creature.transform.position = newPosition;
		}
	}
}