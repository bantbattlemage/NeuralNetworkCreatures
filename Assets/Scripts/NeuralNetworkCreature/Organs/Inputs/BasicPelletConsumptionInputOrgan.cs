using UnityEngine;

public class BasicPelletConsumptionInputOrgan : NeuralNetworkCreatureInputOrgan
{
	public BasicPelletConsumptionInputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type) : base(creature, type)
	{
		_creature = creature;
		Type = type;
		creature.OnCreatureCollision += OnCollision;

		NeuralNetworkCreatureInputSensor sensor = new NeuralNetworkCreatureInputSensor().Initialize("HasEaten", 0);

		Initialize("BasicPelletConsumption", new NeuralNetworkCreatureInputSensor[] { sensor });
	}

	private void OnCollision(Collision collision)
	{
		if (collision.collider.CompareTag("Pellet"))
		{
			_creature.Network.Fitness++;
			_sensors["HasEaten"].SetValue(1);
			_creature.ProcessNetworkInputs();
			Object.Destroy(collision.gameObject);
		}
	}

	public override void UpdateSensors()
	{
		_sensors["HasEaten"].SetValue(0);
	}

	public override NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		return base.CreateDeepCopy();
	}
}