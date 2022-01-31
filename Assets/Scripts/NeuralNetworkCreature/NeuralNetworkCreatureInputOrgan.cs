using System.Collections.Generic;

public class NeuralNetworkCreatureInputOrgan : NeuralNetworkCreatureOrgan, INeuralNetworkCreatureInputOrgan
{
	protected Dictionary<string, NeuralNetworkCreatureInputSensor> _sensors = new Dictionary<string, NeuralNetworkCreatureInputSensor>();

	public NeuralNetworkCreatureInputOrgan Initialize(string name, NeuralNetworkCreatureInputSensor[] sensors)
	{
		Name = name;

		for (int i = 0; i < sensors.Length; i++)
		{
			_sensors.Add(sensors[i].Name, sensors[i]);
		}

		return this;
	}

	/// <summary>
	/// Run the organs function to update it's sensor values. Called when the creature's NeuralNetwork is processed. (Empty if organ does not define function)
	/// </summary>
	public virtual void UpdateSensors()
	{

	}

	public int SensorCount { get { return _sensors.Count; } }

	/// <summary>
	/// Returns the output values of all of the sensors.
	/// </summary>
	public float[] GetInputValues()
	{
		float[] output = new float[_sensors.Count];

		int i = 0;
		foreach(KeyValuePair<string, NeuralNetworkCreatureInputSensor> kvp in _sensors)
		{
			output[i] = kvp.Value.GetValue();
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
			_sensors.Add(sensorName, new NeuralNetworkCreatureInputSensor());
		}

		_sensors[sensorName].SetValue(value);

		return value;
	}
}