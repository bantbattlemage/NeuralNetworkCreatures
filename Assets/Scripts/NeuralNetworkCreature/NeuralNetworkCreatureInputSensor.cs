public class NeuralNetworkCreatureInputSensor : INeuralNetworkCreatureVariable
{
	public string Name { get; private set; }

	private float _value;

	public NeuralNetworkCreatureInputSensor Initialize(string name, float value = 0)
	{
		Name = name;
		_value = value;

		return this;
	}

	public float GetValue()
	{
		return _value;
	}

	public float SetValue(float value)
	{
		_value = value;
		return _value;
	}
}