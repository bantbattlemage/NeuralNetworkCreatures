using UnityEngine;

public class BasicRotationOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public override void Process()
	{
		Rotate();
	}

	private void Rotate()
	{
		_creature.transform.Rotate(0, MutatableVariable.Value * GetOutputValue(), 0, Space.World);
	}
}