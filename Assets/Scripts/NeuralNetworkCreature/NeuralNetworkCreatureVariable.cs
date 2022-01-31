public class NeuralNetworkCreatureVariable : INeuralNetworkCreatureVariable
{
	protected string _name;
	protected float _value;

	public virtual float GetValue()
	{
		return _value;
	}

	public virtual float SetValue(float value)
	{
		_value = value;
		return _value;
	}

	public virtual float SetValue(float value, bool emitValue = true)
	{
		_value = value;
		return _value;
	}

	public string GetName()
	{
		return _name;
	}

	public string SetName(string name)
	{
		_name = name;
		return _name;
	}
}