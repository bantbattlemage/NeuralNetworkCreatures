using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAwarenessInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private NeuralNetworkCreature _creature;

	public SpatialAwarenessInputOrgan(NeuralNetworkCreature creature)
	{
		_creature = creature;

		NeuralNetworkCreatureInputSensor[] directionalSensors = new NeuralNetworkCreatureInputSensor[4];

		directionalSensors[0] = new NeuralNetworkCreatureInputSensor().Initialize("X", _creature.transform.position.x);
		directionalSensors[1] = new NeuralNetworkCreatureInputSensor().Initialize("Y", _creature.transform.position.y);
		directionalSensors[2] = new NeuralNetworkCreatureInputSensor().Initialize("Z", _creature.transform.position.z);
		directionalSensors[3] = new NeuralNetworkCreatureInputSensor().Initialize("R", _creature.transform.rotation.y);

		Initialize("SpatialAwareness", directionalSensors);
	}

	public override void UpdateSensors()
	{
		foreach (KeyValuePair<string, NeuralNetworkCreatureInputSensor> kvp in _sensors)
		{
			switch (kvp.Value.Name)
			{
				case "X":
					kvp.Value.SetValue(_creature.transform.position.x);
					break;
				case "Y":
					kvp.Value.SetValue(_creature.transform.position.y);
					break;
				case "Z":
					kvp.Value.SetValue(_creature.transform.position.z);
					break;
				case "R":
					kvp.Value.SetValue(_creature.transform.rotation.y);
					break;
				default:
					throw new System.Exception("unknown sensor");
			}
		}
	}
}
