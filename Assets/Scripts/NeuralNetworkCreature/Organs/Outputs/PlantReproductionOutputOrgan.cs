using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantReproductionOutputOrgan : NeuralNetworkCreatureOutputOrgan
{
	private EnergyStorageInputOrgan _energyStorageReference;

	private float _energyToReproduce { get { return MutatableVariable.Value * 3f; } }
	private float _energyStored = 0f;

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if(variables == null || variables.Count == 0)
		{
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

		if(_energyStorageReference.GetEnergy() >= MutatableVariable.Value * outputValue)
		{
			//Debug.LogWarning(_energyStorage.GetEnergy());
			_energyStored += _energyStorageReference.TakeEnergy(MutatableVariable.Value * outputValue);

			if(_energyStored >= _energyToReproduce)
			{
				_energyStored = 0;
				NeuralNetworkCreature child = _creature.Reproduce(GameController.Instance.GetRandomCreature(), GameController.Instance.GetRandomWorldCoordinates(5, 5));
				GameController.Instance.AddCreature(child);
			}
		}
	}
}
