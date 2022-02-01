
using UnityEngine;

public class ColorTrait : NeuralNetworkCreatureInheritableTrait
{
	private Color _colorValue;

	public ColorTrait(NeuralNetworkCreature creature, Color c) : base (creature)
	{
		_creature = creature;
		_colorValue = c;
		Name = "Color";
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

	public override void MutateTraitValue()
	{
		_colorValue.r += Random.Range(0, 100) < GameController.Instance.MutationChance ? Random.Range(-GameController.Instance.MutationStrength, GameController.Instance.MutationStrength) : 0;
		_colorValue.b += Random.Range(0, 100) < GameController.Instance.MutationChance ? Random.Range(-GameController.Instance.MutationStrength, GameController.Instance.MutationStrength) : 0;
		_colorValue.g += Random.Range(0, 100) < GameController.Instance.MutationChance ? Random.Range(-GameController.Instance.MutationStrength, GameController.Instance.MutationStrength) : 0;
	}
}