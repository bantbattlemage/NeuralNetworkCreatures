[System.Serializable]
public class NeuralNetworkCreatureVariable : INeuralNetworkCreatureVariable
{
	public string Name;

	private float _variableValue;

	public float Value;
	//public virtual float Value
	//{
	//	get
	//	{
	//		return _variableValue;
	//	}
	//	set
	//	{
	//		if (value > Max)
	//		{
	//			_variableValue = Max;
	//		}
	//		else if (value < Min)
	//		{
	//			_variableValue = Min;
	//		}
	//		else
	//		{
	//			_variableValue = value;
	//		}
	//	}
	//}

	public float Max;
	public float Min;

	public static NeuralNetworkCreatureVariable operator +(NeuralNetworkCreatureVariable a, NeuralNetworkCreatureVariable b) => new NeuralNetworkCreatureVariable(a.Name, a.Value + b.Value);
	public static NeuralNetworkCreatureVariable operator -(NeuralNetworkCreatureVariable a, NeuralNetworkCreatureVariable b) => new NeuralNetworkCreatureVariable(a.Name, a.Value - b.Value);
	public static NeuralNetworkCreatureVariable operator *(NeuralNetworkCreatureVariable a, NeuralNetworkCreatureVariable b) => new NeuralNetworkCreatureVariable(a.Name, a.Value * b.Value);
	public static NeuralNetworkCreatureVariable operator /(NeuralNetworkCreatureVariable a, NeuralNetworkCreatureVariable b) => new NeuralNetworkCreatureVariable(a.Name, a.Value / b.Value);
	public static NeuralNetworkCreatureVariable operator +(NeuralNetworkCreatureVariable a, float b) => new NeuralNetworkCreatureVariable(a.Name, a.Value + b);
	public static NeuralNetworkCreatureVariable operator -(NeuralNetworkCreatureVariable a, float b) => new NeuralNetworkCreatureVariable(a.Name, a.Value - b);
	public static NeuralNetworkCreatureVariable operator *(NeuralNetworkCreatureVariable a, float b) => new NeuralNetworkCreatureVariable(a.Name, a.Value * b);
	public static NeuralNetworkCreatureVariable operator /(NeuralNetworkCreatureVariable a, float b) => new NeuralNetworkCreatureVariable(a.Name, a.Value / b);

	public NeuralNetworkCreatureVariable()
	{

	}

	public NeuralNetworkCreatureVariable(string name = null, float value = 0, float minValue = float.MinValue, float maxValue = float.MaxValue)
	{
		Name = name;
		Min = minValue;
		Max = maxValue;
		Value = value;
	}

	public NeuralNetworkCreatureVariable Copy()
	{
		return new NeuralNetworkCreatureVariable(Name, Value, Min, Max);
	}
}