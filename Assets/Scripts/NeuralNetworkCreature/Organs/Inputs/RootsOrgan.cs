using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsOrgan : NeuralNetworkCreatureInputOrgan
{
	private EnergyStorageInputOrgan _energyStorageReference;
	private SpatialAwarenessInputOrgan _spatialAwarenessReference;

	public override float VariableMinValue { get { return 0f; } }
	public override float VariableMaxValue { get { return 1f; } }
	public static float RootsEfficiencyConstant { get { return 0.75f; } }
	public static float SoilReceptorMinValue { get { return 0f; } }
	public static float SoilReceptorMaxValue { get { return 1f; } }

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if (variables == null || variables.Count == 0)
		{
			NeuralNetworkCreatureVariable soilReceptor = new NeuralNetworkCreatureVariable("soilReceptor", SoilReceptorMinValue, SoilReceptorMaxValue, SoilReceptorMaxValue);

			OrganVariables.Add(soilReceptor.Name, soilReceptor);
		}
	}

	public override void UpdateSensors()
	{
		if (_spatialAwarenessReference == null)
		{
			_spatialAwarenessReference = _creature.InputOrgans["SpatialAwareness"] as SpatialAwarenessInputOrgan;
		}

		Vector2Int worldPosition = _spatialAwarenessReference.GetWorldCoordinatesInt();

		float requestedEnergy = Mathf.Clamp(MutatableVariable.Value * RootsEfficiencyConstant, SoilReceptorMinValue, SoilReceptorMaxValue); 
		float extractedEnergy = GameController.Instance.World.GetWorldTile(worldPosition).ExtractSoilEnergy(requestedEnergy);

		if (_energyStorageReference == null)
		{
			_energyStorageReference = _creature.InputOrgans["EnergyStorage"] as EnergyStorageInputOrgan;
		}

		_energyStorageReference.StoreEnergy(extractedEnergy);
		OrganVariables["soilReceptor"].Value = extractedEnergy;
	}
}
