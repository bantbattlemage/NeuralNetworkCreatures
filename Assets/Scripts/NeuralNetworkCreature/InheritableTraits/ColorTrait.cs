
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorTrait : NeuralNetworkCreatureInheritableTrait
{
	public Color ColorValue;

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);
		ColorValue = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}

	public override NeuralNetworkCreatureInheritableTrait CreateDeepCopy(NeuralNetworkCreature creature)
	{
		ColorTrait copy = new ColorTrait();
		copy.Initialize(creature, Type, OrganVariables.Values.ToList());
		copy.ColorValue = new Color(ColorValue.r, ColorValue.g, ColorValue.b);

		return copy;
	}

	public override void ApplyTraitValue()
	{
		MeshRenderer r = _creature.transform.GetComponent<MeshRenderer>();

		if(r.material != null)
		{
			_creature.transform.GetComponent<MeshRenderer>().material.SetColor("_Color", ColorValue);
		}
	}

	public override void Mutate()
	{
		ColorValue.r += GameController.Instance.RollMutationFactor();
		ColorValue.b += GameController.Instance.RollMutationFactor();
		ColorValue.g += GameController.Instance.RollMutationFactor();
	}
}