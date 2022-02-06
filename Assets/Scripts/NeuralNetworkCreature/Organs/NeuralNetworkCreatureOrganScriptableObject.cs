using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOrganScriptableObject", menuName = "ScriptableObjects/OrganScriptableObject", order = 1)]
public class NeuralNetworkCreatureOrganScriptableObject : ScriptableObject
{
	public NeuralNetworkCreatureOrganType Organ;
	public int ConfigurableInt;
	public float ConfigurableFloat;

	public NeuralNetworkCreatureOrgan Instantiate(NeuralNetworkCreature creature)
	{
		NeuralNetworkCreatureOrgan newOrgan;
		List<NeuralNetworkCreatureVariable> variables = new List<NeuralNetworkCreatureVariable>();

		switch (Organ)
		{
			case NeuralNetworkCreatureOrganType.Heartbeat:
				newOrgan = new HeartbeatInputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				break;
			case NeuralNetworkCreatureOrganType.SpatialAwareness:
				newOrgan = new SpatialAwarenessInputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				break;
			case NeuralNetworkCreatureOrganType.BasicMovement:
				newOrgan = new BasicMovementOutputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				newOrgan.MutatableVariable.Value = ConfigurableFloat;
				break;
			case NeuralNetworkCreatureOrganType.BasicRotation:
				newOrgan = new BasicRotationOutputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				newOrgan.MutatableVariable.Value = ConfigurableFloat;
				break;
			case NeuralNetworkCreatureOrganType.BasicVision:
				newOrgan = new BasicVisionInputOrgan();
				variables.Add(new NeuralNetworkCreatureVariable("0", 0));
				newOrgan.Initialize(creature, Organ, variables);
				newOrgan.MutatableVariable.Value = ConfigurableFloat;
				break;
			case NeuralNetworkCreatureOrganType.BasicPelletConsumption:
				newOrgan = new BasicPelletConsumptionInputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				break;
			case NeuralNetworkCreatureOrganType.Photosynthesis:
				newOrgan = new PhotosynthesisInputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				break;
			case NeuralNetworkCreatureOrganType.EnergyStorage:
				newOrgan = new EnergyStorageInputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				break;
			case NeuralNetworkCreatureOrganType.PlantGrowth:
				newOrgan = new PlantGrowthOutputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				break;
			case NeuralNetworkCreatureOrganType.PlantReproduction:
				newOrgan = new PlantReproductionOutputOrgan();
				newOrgan.Initialize(creature, Organ, variables);
				newOrgan.MutatableVariable.Value = Random.Range(2f, 8f);
				break;
			default:
				throw new System.Exception("No Organ type defined");
		}

		return newOrgan;
	}

	public NeuralNetworkCreatureInheritableTrait Instantiate(NeuralNetworkCreature creature, bool isTrait)
	{
		if(isTrait == false)
		{
			throw new System.Exception("Call other Instantiate function to get an Organ that is not a Trait");
		}

		NeuralNetworkCreatureInheritableTrait newOrgan = null;
		List<NeuralNetworkCreatureVariable> variables = new List<NeuralNetworkCreatureVariable>();

		switch (Organ)
		{
			case NeuralNetworkCreatureOrganType.ColorTrait:
				newOrgan = new ColorTrait();
				break;
			default:
				throw new System.Exception("No Organ type defined");
		}

		newOrgan.Initialize(creature, Organ, variables);

		return newOrgan;
	}
}