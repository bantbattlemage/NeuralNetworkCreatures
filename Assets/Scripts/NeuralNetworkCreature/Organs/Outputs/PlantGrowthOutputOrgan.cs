using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowthOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public static float MaxPlantGrowthHeight { get { return 100f; } }
	public static float MinPlantGrowthHeight { get { return 0.1f; } }
	public static float PlantGrowthConstant { get { return 0.001f; } }

	public float Height = 0.1f;
	private EnergyStorageInputOrgan _energyStorageReference;

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if (variables == null || variables.Count == 0)
		{
			//NeuralNetworkCreatureVariable height = new NeuralNetworkCreatureVariable("Height", _creature.transform.localScale.y, 0.1f, 100f);
			//OrganVariables.Add(height.Name, height);
		}
	}

	public override void Process()
	{
		float outputValue = GetOutputValue();

		if (outputValue <= 0)
		{
			return;
		}

		if (_energyStorageReference == null)
		{
			_energyStorageReference = _creature.InputOrgans["EnergyStorage"] as EnergyStorageInputOrgan;
		}

		float energy = _energyStorageReference.TakeEnergy(Mathf.Abs(outputValue * _creature.transform.localScale.y));
		float growthFactor = (energy * MutatableVariable.Value * PlantGrowthConstant) / Mathf.Pow(_creature.transform.localScale.y, 2);

		Height = Mathf.Clamp(_creature.transform.localScale.y + growthFactor, MinPlantGrowthHeight, MaxPlantGrowthHeight);

		_creature.transform.localScale = new Vector3(_creature.transform.localScale.x, Height, _creature.transform.localScale.z);
		_creature.transform.localPosition = new Vector3(_creature.transform.position.x, _creature.transform.localScale.y, _creature.transform.position.z);
	}
}
