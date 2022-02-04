using System.Collections.Generic;
using System.Linq;

/// <summary>
/// An OutputOrgan recieves input data from the organ's creature's NeuralNetwork and uses that data to perform some function or action.
/// </summary>
public class NeuralNetworkCreatureOutputOrgan : NeuralNetworkCreatureOrgan
{
	public NeuralNetworkCreatureVariable OutputValue { get; protected set; }

	/// <summary>
	/// Iniitialize the organ with the given name.
	/// </summary>
	public override void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		base.Initialize(creature, type, variables);
		OutputValue = new NeuralNetworkCreatureVariable("OutputValue");
	}

	public override NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		NeuralNetworkCreatureOutputOrgan copy = new NeuralNetworkCreatureOutputOrgan();
		copy.Initialize(_creature, Type, OrganVariables.Values.ToList());
		copy.MutatableVariable = MutatableVariable.Copy();
		copy.OutputValue = OutputValue.Copy();
		return copy;
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
	public virtual float GetOutputValue()
	{
		return OutputValue.Value;
	}

	/// <summary>
	/// Set the value of the emitter.
	/// </summary>
	public virtual void SetOutputValue(float value)
	{
		OutputValue.Value = value;
	}
}
