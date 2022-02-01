using UnityEngine;

public class BasicPelletConsumptionInputOrgan : NeuralNetworkCreatureInputOrgan
{
	public BasicPelletConsumptionInputOrgan(NeuralNetworkCreature creature) : base(creature)
	{
		_creature = creature;
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
			GameObject.Destroy(collision.gameObject);
		}
	}

	public override void UpdateSensors()
	{
		_sensors["HasEaten"].SetValue(0);
	}
}