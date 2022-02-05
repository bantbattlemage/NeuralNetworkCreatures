
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorTrait : NeuralNetworkCreatureInheritableTrait
{
	private Color _colorValue;

	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);

		if(variables == null || variables.Count == 0)
		{
			OrganVariables.Add("R", new NeuralNetworkCreatureVariable("R", Random.Range(0f, 1f)));
			OrganVariables.Add("G", new NeuralNetworkCreatureVariable("G", Random.Range(0f, 1f)));
			OrganVariables.Add("B", new NeuralNetworkCreatureVariable("B", Random.Range(0f, 1f)));
		}

		_colorValue = new Color(OrganVariables["R"].Value, OrganVariables["G"].Value, OrganVariables["B"].Value);
	}

	public override NeuralNetworkCreatureOrgan CreateDeepCopy(NeuralNetworkCreature creature)
	{
		ColorTrait copy = new ColorTrait();
		copy.Initialize(creature, Type, OrganVariables.Values.ToList());
		copy._colorValue = new Color(_colorValue.r, _colorValue.g, _colorValue.b);
		ApplyTraitValue();

		return copy;
	}

	public override void ApplyTraitValue()
	{
		MeshRenderer r = _creature.transform.GetComponent<MeshRenderer>();

		if(r.material != null)
		{
			_creature.transform.GetComponent<MeshRenderer>().material.SetColor("_Color", _colorValue);
		}
	}

	public override void Mutate()
	{
		OrganVariables["R"].Value += GameController.Instance.RollMutationFactor();
		OrganVariables["G"].Value += GameController.Instance.RollMutationFactor();
		OrganVariables["B"].Value += GameController.Instance.RollMutationFactor();

		_colorValue = new Color(OrganVariables["R"].Value, OrganVariables["G"].Value, OrganVariables["B"].Value);
	}
}