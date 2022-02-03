using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The HeartbeatInputOrgan gives the NeuralNetwork two input values; a ticking lifetime and the sin wave of that lifetime.
/// </summary>
public class HeartbeatInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private float _lifetime = 0;

	public HeartbeatInputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type) : base(creature, type)
	{
		_creature = creature;
		Type = type;

		NeuralNetworkCreatureVariable[] heartbeat = new NeuralNetworkCreatureVariable[2];

		heartbeat[0] = new NeuralNetworkCreatureVariable("Lifetime", _lifetime);
		heartbeat[1] = new NeuralNetworkCreatureVariable("Rhythm", Mathf.Sin(_lifetime));

		Initialize("Heartbeat", heartbeat);
	}

	public override void UpdateSensors()
	{
		_sensors["Lifetime"].VariableValue = _lifetime;
		_sensors["Rhythm"].VariableValue = Mathf.Sin(_lifetime);
		_lifetime++;
	}
}