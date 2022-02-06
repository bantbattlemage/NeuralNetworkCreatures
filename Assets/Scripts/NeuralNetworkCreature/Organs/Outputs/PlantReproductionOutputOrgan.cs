using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantReproductionOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	private EnergyStorageInputOrgan _energyStorageReference;

	private float _energyToReproduce { get { return  _energyStorageReference.StorageCapacity * ReproductionEnergyRequirementConstant; } }
	private float _energyStored;

	public override float VariableMinValue { get { return 1f; } }
	public override float VariableMaxValue { get { return 5f; } }
	public static float ReproductionEnergyRequirementConstant { get { return 0.5f; } }

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		_energyStored = 0;

		if(variables == null || variables.Count == 0)
		{
			//MutatableVariable = new NeuralNetworkCreatureVariable("MutatableVariable", VariableMinValue, VariableMinValue, VariableMaxValue);
			//NeuralNetworkCreatureVariable increment = new NeuralNetworkCreatureVariable("Increment", 1f, 0.1f, 100f);
			//OrganVariables.Add(increment.Name, increment);
		}
	}

	public override void Process()
	{
		float outputValue = GetOutputValue();

		if(outputValue <= 0)
		{
			return;
		}

		if (_energyStorageReference == null)
		{
			_energyStorageReference = _creature.InputOrgans["EnergyStorage"] as EnergyStorageInputOrgan;
		}

		if(_energyStorageReference.GetEnergy() >= outputValue)
		{
			_energyStored += _energyStorageReference.TakeEnergy(outputValue);

			if(_energyStored >= _energyToReproduce)
			{
				_energyStored = 0;
				NeuralNetworkCreature child = _creature.Reproduce(GameController.Instance.GetRandomCreature(), GameController.Instance.GetRandomWorldCoordinates());
				GameController.Instance.AddCreature(child);
			}
		}
	}
}
