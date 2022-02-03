using System.Collections.Generic;

/// <summary>
/// An OutputOrgan recieves input data from the organ's creature's NeuralNetwork and uses that data to perform some function or action.
/// </summary>
public class NeuralNetworkCreatureOutputOrgan : NeuralNetworkCreatureOrgan
{
	public NeuralNetworkCreatureOutputEmitter Emitter { get; private set; }

	public NeuralNetworkCreatureOutputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type) : base(creature, type)
	{
		_creature = creature;
		Type = type;
	}

	/// <summary>
	/// Iniitialize the organ with the given name.
	/// </summary>
	public NeuralNetworkCreatureOutputOrgan Initialize(string name)
	{
		Name = name;
		Emitter = new NeuralNetworkCreatureOutputEmitter();
		Emitter.Initialize(name);

		return this;
	}

	/// <summary>
	/// Run the organs function to perform an action based on it's OutputValue. Called when the creature's NeuralNetwork is processed. (Empty if organ implementation does not define function)
	/// </summary>
	public virtual void Process()
	{

	}

	/// <summary>
	/// Return the current value of the associated NeuralNetwork output node
	/// </summary>
	public float GetOutputValue()
	{
		return Emitter.VariableValue;
	}

	/// <summary>
	/// Set the value of the emitter.
	/// </summary>
	public void SetOutputValue(float value)
	{
		Emitter.VariableValue = value;
	}
}
