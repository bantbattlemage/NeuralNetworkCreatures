public interface INeuralNetworkCreatureInputOrgan : INeuralNetworkCreatureOrgan
{
	NeuralNetworkCreatureInputOrgan Initialize(string name, NeuralNetworkCreatureInputSensor[] sensors);
	public float[] GetInputValues();
	public float SetInputValue(string name, float value);
	public void UpdateSensors();
}