using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowthOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	public float Height = 0.1f;
	private EnergyStorageInputOrgan _energyStorageReference;
	private GameObject _bodyBaseReference;

	public static float MaxPlantGrowthHeight { get { return 100f; } }
	public static float MinPlantGrowthHeight { get { return 0.1f; } }
	public static float PlantGrowthEfficiencyConstant { get { return 0.1f; } }

	public override float VariableMinValue { get { return 0f; } }
	public override float VariableMaxValue { get { return 1f; }}

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if (variables == null || variables.Count == 0)
		{
			//NeuralNetworkCreatureVariable height = new NeuralNetworkCreatureVariable("Height", _creature.transform.localScale.y, 0.1f, 100f);
			//OrganVariables.Add(height.Name, height);
		}

		_bodyBaseReference = _creature.transform.Find("PlantBodyBase").gameObject;
		_bodyBaseReference.transform.localScale = new Vector3(_bodyBaseReference.transform.localScale.x, Height, _bodyBaseReference.transform.localScale.z);
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

		float energy = _energyStorageReference.TakeEnergy(outputValue * _bodyBaseReference.transform.localScale.y);
		float growthFactor = (energy * MutatableVariable.Value * PlantGrowthEfficiencyConstant) / Mathf.Pow(_bodyBaseReference.transform.localScale.y, 2);

		Height = Mathf.Clamp(_bodyBaseReference.transform.localScale.y + growthFactor, MinPlantGrowthHeight, MaxPlantGrowthHeight);

		_bodyBaseReference.transform.localScale = new Vector3(_bodyBaseReference.transform.localScale.x, Height, _bodyBaseReference.transform.localScale.z);
		_bodyBaseReference.transform.position = new Vector3(_bodyBaseReference.transform.position.x, Height, _bodyBaseReference.transform.position.z);
	}
}