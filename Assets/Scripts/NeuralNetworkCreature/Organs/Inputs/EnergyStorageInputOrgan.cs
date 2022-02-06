using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An essential organ for all creatures to store energy. MutatableVariable affects energy storage capacy.
/// </summary>
public class EnergyStorageInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private float _storedEnergy;
	private float _storageCapacity;


	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		//if(variables == null || variables.Count == 0)
		//{
		//	NeuralNetworkCreatureVariable storage = new NeuralNetworkCreatureVariable("StoredEnergy", 1f);
		//	storage.Min = 0;
		//	storage.Max = 100 + (2 * MutatableVariable.Value);
		//	OrganVariables.Add(storage.Name, storage);
		//}

		_storedEnergy = 1f;
	}

	public override void UpdateSensors()
	{

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

		_storedEnergy += energyAmount;
		//OrganVariables["StoredEnergy"].Value = energyAmount;
	}

	public float TakeEnergy(float requestedAmount)
	{
		float output;

		if(requestedAmount < 0)
		{
			throw new System.Exception("negative energy!");
		}

		//if(energyAmount >= OrganVariables["StoredEnergy"].Value)
		//{
		//	energyAmount = OrganVariables["StoredEnergy"].Value;
		//}

		//OrganVariables["StoredEnergy"].Value -= energyAmount;

		if (requestedAmount >= _storedEnergy)
		{
			requestedAmount = _storedEnergy;
		}

		//OrganVariables["StoredEnergy"].Value -= energyAmount;

		output = requestedAmount;
		_storedEnergy -= requestedAmount;

		return output;
	}
}