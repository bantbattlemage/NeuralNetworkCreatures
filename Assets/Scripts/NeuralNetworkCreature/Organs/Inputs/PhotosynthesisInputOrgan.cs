using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotosynthesisInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private EnergyStorageInputOrgan _energyStorageReference;
	private SpatialAwarenessInputOrgan _spatialAwarenessReference;

	public override float VariableMinValue { get { return 0f; } }
	public override float VariableMaxValue { get { return 1f; } }
	public static float PhotosynthesisEfficiencyConstant { get { return 0.1f; } }
	public static float PhotoreceptorMinValue { get { return 0f; } }
	public static float PhotoreceptorMaxValue { get { return 1f; } }

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if (variables == null || variables.Count == 0)
		{
			NeuralNetworkCreatureVariable photoreceptor = new NeuralNetworkCreatureVariable("photoreceptor", PhotoreceptorMinValue, PhotoreceptorMinValue, PhotoreceptorMaxValue);

			OrganVariables.Add(photoreceptor.Name, photoreceptor);
		}
	}

	public override void UpdateSensors()
	{
		if (_spatialAwarenessReference == null)
		{
			_spatialAwarenessReference = _creature.InputOrgans["SpatialAwareness"] as SpatialAwarenessInputOrgan;
		}

		float[] positionValues = _creature.InputOrgans["SpatialAwareness"].GetInputValues();

		Vector2Int worldPosition = new Vector2Int(Mathf.FloorToInt(positionValues[0]), Mathf.FloorToInt(positionValues[1]));
		float sunlightValue =  Mathf.Clamp(GameController.Instance.World.GetSunlightValue(worldPosition) * MutatableVariable.Value * PhotosynthesisEfficiencyConstant, PhotoreceptorMinValue, PhotoreceptorMaxValue);

		//	store energy equal to the photoreceptor value * this organ's mutatable varaible
		OrganVariables["photoreceptor"].Value = sunlightValue;

		if(_energyStorageReference == null)
		{
			_energyStorageReference = _creature.InputOrgans["EnergyStorage"] as EnergyStorageInputOrgan;
		}

		_energyStorageReference.StoreEnergy(OrganVariables["photoreceptor"].Value);
	}
}
