using System.Collections.Generic;

/// <summary>
/// An OutputOrgan recieves input data from the organ's creature's NeuralNetwork and uses that data to perform some function or action.
/// </summary>
public class NeuralNetworkCreatureOutputOrgan : NeuralNetworkCreatureOrgan
{
	public NeuralNetworkCreatureOutputEmitter Emitter { get; private set; }

	public NeuralNetworkCreatureOutputOrgan(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type) : base(creature, type)
	{

	}

	/// <summary>
	/// Iniitialize the organ with the given name.
	/// </summary>
	public NeuralNetworkCreatureOutputOrgan Initialize(string name)
	{
		SetName(name);
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
		return Emitter.GetValue();
	}

	/// <summary>
	/// Set the value of emitter and emit the value if emitValue = true
	/// </summary>
	public float SetOutputValue(float value, bool emitValue = false)
	{
		value = Emitter.SetValue(value, emitValue);
		return value;
	}

	/// <summary>
	/// Set the value of the emitter. Does NOT emit the event.
	/// </summary>
	public float SetOutputValue(float value)
	{
		return SetOutputValue(value, false);
	}

	public override NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		NeuralNetworkCreatureOutputOrgan copy = new NeuralNetworkCreatureOutputOrgan(_creature, Type);
		copy.SetValue(_value);
		copy.SetName(_name);
		return copy;
	}
}
