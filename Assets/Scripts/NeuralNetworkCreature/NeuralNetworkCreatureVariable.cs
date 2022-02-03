public class NeuralNetworkCreatureVariable : INeuralNetworkCreatureVariable
{
	public virtual string Name { get; protected set; }

	public virtual float VariableValue { get; set; }

	public NeuralNetworkCreatureVariable(string name = null, float value = 0)
	{
		Name = name;
		VariableValue = value;
	}

	public NeuralNetworkCreatureVariable CopyCreatureVariable()
	{
		return new NeuralNetworkCreatureVariable(Name, VariableValue);
	}
}