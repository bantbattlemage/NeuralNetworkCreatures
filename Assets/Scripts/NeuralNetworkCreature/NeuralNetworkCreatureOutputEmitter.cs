public class NeuralNetworkCreatureOutputEmitter : INeuralNetworkCreatureVariable
{
	public string Name { get; private set; }

	private float _value;

	public delegate float EmitValueEvent();
	private EmitValueEvent EmitValue;

	public void Initialize(string name, float value = 0)
	{
		Name = name;
		_value = value;
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

	public float SetValue(float value, bool emitValue = true)
	{
		_value = value;

		if(emitValue && EmitValue != null)
		{
			EmitValue();
		}

		return _value;
	}

	public void RegisterOnValueEmissionEvent(EmitValueEvent e)
	{
		EmitValue += e;
	}
}