public interface INeuralNetworkCreatureOutputOrgan : INeuralNetworkCreatureOrgan
{
	public NeuralNetworkCreatureOutputOrgan Initialize(string name);
	public float GetOutputValue();
	public float SetOutputValue(float value);
	public void Process();
}