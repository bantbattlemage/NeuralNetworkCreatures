using System.Collections.Generic;
using UnityEngine;

public class BasicPelletConsumptionInputOrgan : NeuralNetworkCreatureInputOrgan
{
	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		NeuralNetworkCreatureVariable sensor = new NeuralNetworkCreatureVariable("HasEaten", 0);
		OrganVariables.Add(sensor.Name, sensor);

		creature.OnCreatureCollision += OnCollision;
	}

	private void OnCollision(Collision collision)
	{
		if (collision.collider.CompareTag("Pellet"))
		{
			_creature.Network.Fitness++;
			OrganVariables["HasEaten"].Value = 1;
			_creature.ProcessNetworkInputs();
			Object.Destroy(collision.gameObject);
		}
	}

	public override void UpdateSensors()
	{
		OrganVariables["HasEaten"].Value = 0;
	}

	public override NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		return base.CreateDeepCopy();
	}
}