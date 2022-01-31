public interface INeuralNeworkCreatureInheritableTrait
{
	public NeuralNetworkCreatureInheritableTrait Initialize(NeuralNetworkCreature creature);
	public string GetName();
	public string SetName(string name);
	public float GetValue();
	public float SetValue(float value);
	public void ApplyTraitValue();
	public void MutateTraitValue();
}
