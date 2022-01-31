public class NeuralNetworkCreatureOutputEmitter : NeuralNetworkCreatureVariable
{
	public delegate float EmitValueEvent();
	protected EmitValueEvent EmitValue;

	public void Initialize(string name, float value = 0)
	{
		_name = name;
		_value = value;
	}

	public override float SetValue(float value, bool emitValue = true)
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