using System.Collections.Generic;

/// <summary>
/// An InputOrgan contains Sensors that recieve input of some kind from the environment and pass that data into the organ's creature's NeuralNetwork.
/// </summary>
public class NeuralNetworkCreatureInputOrgan : NeuralNetworkCreatureOrgan
{
	protected Dictionary<string, NeuralNetworkCreatureVariable> _sensors = new Dictionary<string, NeuralNetworkCreatureVariable>();

	public int SensorCount { get { return _sensors.Count; } }

	public NeuralNetworkCreatureInputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type) : base(creature, type)
	{
		_creature = creature;
	}

	public NeuralNetworkCreatureInputOrgan Initialize(string name, NeuralNetworkCreatureVariable[] sensors)
	{
		Name = name;

		for (int i = 0; i < sensors.Length; i++)
		{
			_sensors.Add(sensors[i].Name, sensors[i]);
		}

		return this;
	}

	/// <summary>
	/// Run the organs function to update it's sensor values. Called when the creature's NeuralNetwork is processed. (Empty if organ implementation does not define function)
	/// </summary>
	public virtual void UpdateSensors()
	{

	}

	/// <summary>
	/// Returns the output values of all of the sensors.
	/// </summary>
	public float[] GetInputValues()
	{
		float[] output = new float[_sensors.Count];

		int i = 0;
		foreach(KeyValuePair<string, NeuralNetworkCreatureVariable> kvp in _sensors)
		{
			output[i] = kvp.Value.VariableValue;
			i++;
		}

		return output;
	}

	/// <summary>
	/// Set the value of the sensor. If the sensor does not exist, creates it and sets the value.
	/// </summary>
	public float SetInputValue(string sensorName, float value)
	{
		if(!_sensors.ContainsKey(sensorName))
		{
			_sensors.Add(sensorName, new NeuralNetworkCreatureVariable());
		}

		_sensors[sensorName].VariableValue = value;

		return value;
	}
}