
using UnityEngine;

public class ColorTrait : NeuralNetworkCreatureInheritableTrait
{
	private Color _colorValue;

	public ColorTrait(Color c)
	{
		_colorValue = c;
		Name = "Color";
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