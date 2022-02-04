using System.Collections.Generic;

/// <summary>
/// An InputOrgan contains Sensors that recieve input of some kind from the environment and pass that data into the organ's creature's NeuralNetwork.
/// </summary>
public class NeuralNetworkCreatureInputOrgan : NeuralNetworkCreatureOrgan
{
	public int SensorCount { get { return OrganVariables.Values.Count; } }

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
		float[] output = new float[SensorCount];

		int i = 0;
		foreach(NeuralNetworkCreatureVariable v in OrganVariables.Values)
		{
			output[i] = v.Value;
			i++;
		}

		return output;
	}

	/// <summary>
	/// Set the value of the sensor. If the sensor does not exist, creates it and sets the value.
	/// </summary>
	public float SetInputValue(string sensorName, float value)
	{
		if(!OrganVariables.ContainsKey(sensorName))
		{
			OrganVariables.Add(sensorName, new NeuralNetworkCreatureVariable());
		}

		OrganVariables[sensorName].Value = value;

		return value;
	}
}