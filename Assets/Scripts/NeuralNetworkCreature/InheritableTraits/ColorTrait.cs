
using UnityEngine;

public class ColorTrait : NeuralNetworkCreatureInheritableTrait
{
	private Color _colorValue;

	public ColorTrait(NeuralNetworkCreature creature, Color c) : base (creature)
	{
		_creature = creature;
		_colorValue = c;
		SetName("Color");
	}

	public override NeuralNetworkCreatureInheritableTrait CreateDeepCopy(NeuralNetworkCreature creature)
	{
		return new ColorTrait(creature, _colorValue);
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
		_colorValue.r += GameController.Instance.RollMutationFactor();
		_colorValue.b += GameController.Instance.RollMutationFactor();
		_colorValue.g += GameController.Instance.RollMutationFactor();
	}
}