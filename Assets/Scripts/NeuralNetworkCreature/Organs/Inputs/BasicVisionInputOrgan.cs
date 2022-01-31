using UnityEngine;

public class BasicVisionInputOrgan : NeuralNetworkCreatureInputOrgan
{
	private NeuralNetworkCreature _creature;
	private float _visionDistance;

	/// <summary>
	/// The BasicVisionOrgan gives the NeuralNetwork an input value of the distance to any Pellets hit by [sensorCount] rays radiating from the sphere
	/// </summary>
	public BasicVisionInputOrgan(NeuralNetworkCreature creature, int sensorCount, float visionDistance)
	{
		_creature = creature;
		_visionDistance = visionDistance;

		NeuralNetworkCreatureInputSensor[] rayResults = new NeuralNetworkCreatureInputSensor[sensorCount];

		for(int i = 0; i < sensorCount; i++)
		{
			string name = i.ToString();
			rayResults[i] = new NeuralNetworkCreatureInputSensor().Initialize(name, -1);
		}

		Initialize("BasicVision", rayResults);
	}

	public override void UpdateSensors()
	{
		for (int i = 0; i < _sensors.Count; i++)
		{
			Vector3 newVector = Quaternion.AngleAxis(i * (360f/_sensors.Count), Vector3.up) * _creature.transform.forward;
			RaycastHit hit;
			Ray ray = new Ray(_creature.transform.position + newVector, newVector);
			//Debug.DrawRay(ray.origin, ray.direction, Color.red);

			if (Physics.Raycast(ray, out hit, _visionDistance))
			{
				if(hit.transform.CompareTag("Pellet"))
				{
					_sensors[i.ToString()].SetValue(hit.distance);
				}
				else if(hit.transform.CompareTag("NetworkObject") && hit.transform != _creature.transform)
				{
					_sensors[i.ToString()].SetValue(-hit.distance);
				}
				else
				{
					_sensors[i.ToString()].SetValue(0);
				}
			}
			else
			{
				_sensors[i.ToString()].SetValue(0);
			}
		}
	}
}