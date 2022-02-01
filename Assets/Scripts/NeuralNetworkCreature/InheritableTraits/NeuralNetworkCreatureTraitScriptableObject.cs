using UnityEngine;

[CreateAssetMenu(fileName = "NewTraitScriptableObject", menuName = "ScriptableObjects/TraitScriptableObject", order = 1)]
public class NeuralNetworkCreatureTraitScriptableObject : ScriptableObject
{
	public ENeuralNetworkCreatureTraitType Trait;
	public int[] ConfigurableInts;
	public float[] ConfigurableFloats;

	public NeuralNetworkCreatureInheritableTrait Instantiate(NeuralNetworkCreature creature)
	{
		switch (Trait)
		{
			case ENeuralNetworkCreatureTraitType.Color:
				//return new ColorTrait(new Color(ConfigurableFloats[0], ConfigurableFloats[1], ConfigurableFloats[2])).Initialize(creature);
				return new ColorTrait(creature, new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
			default:
				return new NeuralNetworkCreatureInheritableTrait(creature);
		}
	}
}