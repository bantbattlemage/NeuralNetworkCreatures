using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The HeartbeatInputOrgan gives the NeuralNetwork two input values; a ticking lifetime and the sin wave of that lifetime.
/// </summary>
public class HeartbeatInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private float _lifetime = 0;

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if(variables == null || variables.Count == 0)
		{
			NeuralNetworkCreatureVariable[] heartbeat = new NeuralNetworkCreatureVariable[2];
			heartbeat[0] = new NeuralNetworkCreatureVariable("Lifetime", _lifetime);
			heartbeat[1] = new NeuralNetworkCreatureVariable("Rhythm", Mathf.Sin(_lifetime));
			OrganVariables.Add(heartbeat[0].Name, heartbeat[0]);
			OrganVariables.Add(heartbeat[1].Name, heartbeat[1]);
		}
	}

	public override void UpdateSensors()
	{
		OrganVariables["Lifetime"].Value = _lifetime;
		OrganVariables["Rhythm"].Value = Mathf.Sin(_lifetime);
		_lifetime++;
	}
}