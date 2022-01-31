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
				return new ColorTrait(new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f))).Initialize(creature);
			default:
				return new NeuralNetworkCreatureInheritableTrait();
		}
	}
}