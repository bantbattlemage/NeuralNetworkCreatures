public class NeuralNetworkCreatureInputSensor : NeuralNetworkCreatureVariable
{	
	public NeuralNetworkCreatureInputSensor Initialize(string name, float value = 0)
	{
		_name = name;
		_value = value;

		return this;
	}
}