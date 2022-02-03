using UnityEngine;

[CreateAssetMenu(fileName = "NewOrganScriptableObject", menuName = "ScriptableObjects/OrganScriptableObject", order = 1)]
public class NeuralNetworkCreatureOrganScriptableObject : ScriptableObject
{
	public NeuralNetworkCreatureOrganType Organ;
	public int ConfigurableInt;
	public float ConfigurableFloat;

	public NeuralNetworkCreatureOrgan Instantiate(NeuralNetworkCreature creature)
	{
		switch (Organ)
		{
			case NeuralNetworkCreatureOrganType.Heartbeat:
				return new HeartbeatInputOrgan(creature, Organ);
			case NeuralNetworkCreatureOrganType.SpatialAwareness:
				return new SpatialAwarenessInputOrgan(creature, Organ);
			case NeuralNetworkCreatureOrganType.BasicMovement:
				return new BasicMovementOutputOrgan(creature, Organ, ConfigurableFloat);
			case NeuralNetworkCreatureOrganType.BasicRotation:
				return new BasicRotationOutputOrgan(creature, Organ, ConfigurableFloat);
			case NeuralNetworkCreatureOrganType.BasicVision:
				return new BasicVisionInputOrgan(creature, Organ, ConfigurableInt, ConfigurableFloat);
			case NeuralNetworkCreatureOrganType.BasicPelletConsumption:
				return new BasicPelletConsumptionInputOrgan(creature, Organ);
			default:
				throw new System.Exception("No Organ type defined");
		}
	}

	public NeuralNetworkCreatureInheritableTrait Instantiate(NeuralNetworkCreature creature, bool isTrait)
	{
		if(isTrait == false)
		{
			throw new System.Exception("Call other Instantiate function to get an Organ that is not a Trait");
		}

		switch (Organ)
		{
			case NeuralNetworkCreatureOrganType.ColorTrait:
				return new ColorTrait(creature, NeuralNetworkCreatureOrganType.ColorTrait, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
			default:
				throw new System.Exception("No Organ type defined");
		}
	}
}