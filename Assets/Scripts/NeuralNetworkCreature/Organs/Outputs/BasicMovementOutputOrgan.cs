using System.Collections.Generic;
using UnityEngine;

public class BasicMovementOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);
		MutatableVariable.Min = -10f;
		MutatableVariable.Max = 10f;
	}

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