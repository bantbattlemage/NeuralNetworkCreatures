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
				return new HeartbeatInputOrgan(creature);
			case NeuralNetworkCreatureOrganType.SpatialAwareness:
				return new SpatialAwarenessInputOrgan(creature);
			case NeuralNetworkCreatureOrganType.BasicMovement:
				return new BasicMovementOutputOrgan(creature.MovementSpeed, creature);
			case NeuralNetworkCreatureOrganType.BasicRotation:
				return new BasicRotationOutputOrgan(creature.RotationSpeed, creature);
			case NeuralNetworkCreatureOrganType.BasicVision:
				return new BasicVisionInputOrgan(creature, ConfigurableInt, ConfigurableFloat);
			case NeuralNetworkCreatureOrganType.BasicPelletConsumption:
				return new BasicPelletConsumptionInputOrgan(creature);
			case NeuralNetworkCreatureOrganType.GenericInput:
				NeuralNetworkCreatureInputOrgan io = new NeuralNetworkCreatureInputOrgan(creature);
				io.SetName(Random.Range(0f, 10000f).ToString());
				return io;
			case NeuralNetworkCreatureOrganType.GenericOutput:
				NeuralNetworkCreatureOutputOrgan oo = new NeuralNetworkCreatureOutputOrgan(creature);
				oo.SetName(Random.Range(0f, 10000f).ToString());
				return oo;
			default:
				return new NeuralNetworkCreatureOrgan(creature);
		}
	}
}