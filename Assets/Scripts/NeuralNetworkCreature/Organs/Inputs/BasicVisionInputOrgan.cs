using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The BasicVisionOrgan gives the NeuralNetwork an input value of the distance to any Pellets hit by [sensorCount] rays radiating from the sphere
/// </summary>
public class BasicVisionInputOrgan : NeuralNetworkCreatureInputOrgan
{
	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		MutatableVariable.Min = 0;
		MutatableVariable.Max = GameController.Instance.WorldSize.x;

		if(variables == null || variables.Count == 0)
		{
			OrganVariables.Add("0", new NeuralNetworkCreatureVariable("0"));
		}
		//else
		//{
		//	MutatableVariable.Value = variables[0].Value;
		//	OrganVariables = new Dictionary<string, NeuralNetworkCreatureVariable>();

		//	for (int i = 0; i < variables[1].Value; i++)
		//	{
		//		string name = i.ToString();
		//		OrganVariables.Add(name, new NeuralNetworkCreatureVariable(name));
		//	}
		//}
	}

	public override void UpdateSensors()
	{
		for (int i = 0; i < OrganVariables.Count; i++)
		{
			Vector3 newVector = Quaternion.AngleAxis(i * (360f/OrganVariables.Count), Vector3.up) * _creature.transform.forward;
			RaycastHit hit;
			Ray ray = new Ray(_creature.transform.position + newVector, newVector);
			//Debug.DrawRay(ray.origin, ray.direction, Color.red);

			if (Physics.Raycast(ray, out hit, MutatableVariable.Value))
			{
				if(hit.transform.CompareTag("Pellet"))
				{
					OrganVariables[i.ToString()].Value = hit.distance;
				}
				else
				{
					OrganVariables[i.ToString()].Value = 0;
				}
			}
			else
			{
				OrganVariables[i.ToString()].Value = 0;
			}
		}
	}
}