using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafTrait : NeuralNetworkCreatureInheritableTrait
{
	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if (variables == null || variables.Count == 0)
		{
			OrganVariables.Add("NumberPerSection", new NeuralNetworkCreatureVariable("NumberPerSection", Random.Range(1, 5), 1, 4));
		}
	}

	public override void ApplyTraitValue()
	{
		PlantGrowthOutputOrgan growthOrgan = _creature.OutputOrgans["PlantGrowth"] as PlantGrowthOutputOrgan;
		growthOrgan.InitializeLeaves();
	}

	public int NumberOfLeavesPerSection
	{
		get
		{
			return Mathf.FloorToInt(OrganVariables["NumberPerSection"].Value);
		}
	}
}