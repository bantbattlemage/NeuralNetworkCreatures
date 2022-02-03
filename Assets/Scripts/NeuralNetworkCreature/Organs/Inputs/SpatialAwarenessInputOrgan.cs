using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The SpatialAwareness organ gives the NeuralNetwork the creature's spatial coordinates and Y axis rotation.
/// </summary>
public class SpatialAwarenessInputOrgan : NeuralNetworkCreatureInputOrgan
{
	public SpatialAwarenessInputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type) : base(creature, type)
	{
		_creature = creature;
		Type = type;

		NeuralNetworkCreatureVariable[] directionalSensors = new NeuralNetworkCreatureVariable[4];

		directionalSensors[0] = new NeuralNetworkCreatureVariable("X", _creature.transform.position.x);
		directionalSensors[1] = new NeuralNetworkCreatureVariable("Y", _creature.transform.position.y);
		directionalSensors[2] = new NeuralNetworkCreatureVariable("Z", _creature.transform.position.z);
		directionalSensors[3] = new NeuralNetworkCreatureVariable("R", _creature.transform.rotation.y);

		Initialize("SpatialAwareness", directionalSensors);
	}

	public override void UpdateSensors()
	{
		foreach (KeyValuePair<string, NeuralNetworkCreatureVariable> kvp in _sensors)
		{
			switch (kvp.Value.Name)
			{
				case "X":
					kvp.Value.VariableValue = _creature.transform.position.x;
					break;
				case "Y":
					kvp.Value.VariableValue = _creature.transform.position.y;
					break;
				case "Z":
					kvp.Value.VariableValue = _creature.transform.position.z;
					break;
				case "R":
					kvp.Value.VariableValue = _creature.transform.rotation.y;
					break;
				default:
					throw new System.Exception("unknown sensor");
			}
		}
	}
}