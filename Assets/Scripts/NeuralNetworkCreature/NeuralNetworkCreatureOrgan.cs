public class NeuralNetworkCreatureOrgan : INeuralNetworkCreatureOrgan
{
	public string Name { get; protected set; }

	protected NeuralNetworkCreature _creature;

	public NeuralNetworkCreatureOrgan(NeuralNetworkCreature creature)
	{
		_creature = creature;
	}

	public string GetName()
	{
		return Name;
	}

	public void SetName(string name)
	{
		Name = name;
	}
}