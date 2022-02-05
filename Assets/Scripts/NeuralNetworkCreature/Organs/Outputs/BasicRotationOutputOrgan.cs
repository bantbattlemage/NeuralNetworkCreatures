using System.Collections.Generic;
using UnityEngine;

public class BasicRotationOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);
		MutatableVariable.Min = -360f;
		MutatableVariable.Max = 360f;
	}

	public override void Process()
	{
		Rotate();
	}

	private void Rotate()
	{
		_creature.transform.Rotate(0, MutatableVariable.Value * GetOutputValue(), 0, Space.World);
	}
}