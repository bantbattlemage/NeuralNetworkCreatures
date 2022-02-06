using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotosynthesisInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private EnergyStorageInputOrgan _energyStorageReference;
	private SpatialAwarenessInputOrgan _spatialAwarenessReference;
	private const float _photosynthesisConstModifier = 0.5f;

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if (variables == null || variables.Count == 0)
		{
			NeuralNetworkCreatureVariable photoreceptor = new NeuralNetworkCreatureVariable("photoreceptor", 0f, 0f, 10f);

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
		float sunlightValue =  Mathf.Clamp(GameController.Instance.World.GetSunlightValue(worldPosition) * MutatableVariable.Value * _photosynthesisConstModifier, 0f, 10f);

		//	store energy equal to the photoreceptor value * this organ's mutatable varaible
		OrganVariables["photoreceptor"].Value = sunlightValue;

		if(_energyStorageReference == null)
		{
			_energyStorageReference = _creature.InputOrgans["EnergyStorage"] as EnergyStorageInputOrgan;
		}

		_energyStorageReference.StoreEnergy(OrganVariables["photoreceptor"].Value);
	}
}
