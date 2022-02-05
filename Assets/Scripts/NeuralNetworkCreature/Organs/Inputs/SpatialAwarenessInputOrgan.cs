using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The SpatialAwareness organ gives the NeuralNetwork the creature's spatial coordinates and Y axis rotation.
/// </summary>
public class SpatialAwarenessInputOrgan : NeuralNetworkCreatureInputOrgan
{
	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if(variables == null || variables.Count == 0)
		{
			NeuralNetworkCreatureVariable[] directionalSensors = new NeuralNetworkCreatureVariable[4];

			directionalSensors[0] = new NeuralNetworkCreatureVariable("X", _creature.transform.position.x);
			directionalSensors[1] = new NeuralNetworkCreatureVariable("Y", _creature.transform.position.y);
			directionalSensors[2] = new NeuralNetworkCreatureVariable("Z", _creature.transform.position.z);
			directionalSensors[3] = new NeuralNetworkCreatureVariable("R", _creature.transform.rotation.y);

			for (int i = 0; i < directionalSensors.Length; i++)
			{
				OrganVariables.Add(directionalSensors[i].Name, directionalSensors[i]);
			}
		}
	}

	public override void UpdateSensors()
	{
		foreach (NeuralNetworkCreatureVariable sensor in OrganVariables.Values)
		{
			switch (sensor.Name)
			{
				case "X":
					sensor.Value = _creature.transform.position.x;
					break;
				case "Y":
					sensor.Value = _creature.transform.position.y;
					break;
				case "Z":
					sensor.Value = _creature.transform.position.z;
					break;
				case "R":
					sensor.Value = _creature.transform.rotation.y;
					break;
				default:
					throw new System.Exception("unknown sensor");
			}
		}
	}
}