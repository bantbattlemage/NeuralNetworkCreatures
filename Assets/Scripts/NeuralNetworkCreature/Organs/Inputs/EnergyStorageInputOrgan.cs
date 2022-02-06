using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An essential organ for all creatures to store energy. MutatableVariable affects energy storage capacy.
/// </summary>
public class EnergyStorageInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private float _storedEnergy = 0f;
	public float StorageCapacity { get { return EnergyStorageDefaultValue + MutatableVariable.Value; } }
	public float EnergyStorageDefaultValue { get { return 20f; } }

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if (variables == null || variables.Count == 0)
		{
			NeuralNetworkCreatureVariable storage = new NeuralNetworkCreatureVariable("StoredEnergy", 0f);
			storage.Min = 0;
			storage.Max = StorageCapacity;
			OrganVariables.Add(storage.Name, storage);
		}

		_storedEnergy = 1f;
	}

	public override void UpdateSensors()
	{
		OrganVariables["StoredEnergy"].Value = _storedEnergy;
	}

	public float GetEnergy()
	{
		return _storedEnergy;
	}

	public void StoreEnergy(float energyAmount)
	{
		if (energyAmount < 0)
		{
			throw new System.Exception("negative energy!");
		}

		if(_storedEnergy + energyAmount >= StorageCapacity)
		{
			_storedEnergy = StorageCapacity;
		}
		else
		{
			_storedEnergy += energyAmount;
		}
	}

	public float TakeEnergy(float requestedAmount)
	{
		float output;

		if(requestedAmount < 0)
		{
			throw new System.Exception("negative energy!");
		}
		else if(requestedAmount == 0 || _storedEnergy == 0)
		{
			return 0;
		}

		if (requestedAmount >= _storedEnergy)
		{
			requestedAmount = _storedEnergy;
		}

		output = requestedAmount;
		_storedEnergy -= requestedAmount;

		return output;
	}
}