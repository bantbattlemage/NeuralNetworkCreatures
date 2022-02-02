using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The HeartbeatInputOrgan gives the NeuralNetwork two input values; a ticking lifetime and the sin wave of that lifetime.
/// </summary>
public class HeartbeatInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private float _lifetime = 0;

	public HeartbeatInputOrgan(NeuralNetworkCreature creature) : base(creature)
	{
		NeuralNetworkCreatureInputSensor[] heartbeat = new NeuralNetworkCreatureInputSensor[2];

		heartbeat[0] = new NeuralNetworkCreatureInputSensor().Initialize("Lifetime", _lifetime);
		heartbeat[1] = new NeuralNetworkCreatureInputSensor().Initialize("Rhythm", Mathf.Sin(_lifetime));

		Initialize("Heartbeat", heartbeat);
	}

	public override void UpdateSensors()
	{
		_sensors["Lifetime"].SetValue(_lifetime);
		_sensors["Rhythm"].SetValue(Mathf.Sin(_lifetime));
		_lifetime++;
	}
}