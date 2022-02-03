public class NeuralNetworkCreatureOutputEmitter : NeuralNetworkCreatureVariable
{
	public delegate float EmitValueEvent();
	protected EmitValueEvent EmitValue;

	public void Initialize(string name, float value = 0)
	{
		Name = name;
		VariableValue = value;
	}

	public void RegisterOnValueEmissionEvent(EmitValueEvent e)
	{
		EmitValue += e;
	}
}