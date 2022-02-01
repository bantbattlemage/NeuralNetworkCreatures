using System.Collections.Generic;

public class NeuralNetworkCreatureOutputOrgan : NeuralNetworkCreatureOrgan, INeuralNetworkCreatureOutputOrgan
{
	public NeuralNetworkCreatureOutputEmitter Emitter { get; private set; }

	public NeuralNetworkCreatureOutputOrgan(NeuralNetworkCreature creature) : base(creature)
	{

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
	/// Return the current value of the emitter;
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
}
