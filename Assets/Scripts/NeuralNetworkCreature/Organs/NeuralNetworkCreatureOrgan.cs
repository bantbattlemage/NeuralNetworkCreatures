using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class NeuralNetworkCreatureOrgan : INeuralNetworkCreatureOrgan
{
	public string Name;
	public NeuralNetworkCreatureOrganType Type;
	public NeuralNetworkCreatureVariable MutatableVariable;
	public Dictionary<string, NeuralNetworkCreatureVariable> OrganVariables;

	protected NeuralNetworkCreature _creature;

	public static float DefaultMutatableVariableRange { get { return 5f; } }
	public virtual float VariableMinValue { get { return -DefaultMutatableVariableRange; } }
	public virtual float VariableMaxValue { get { return DefaultMutatableVariableRange; } }

	public virtual void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		_creature = creature;
		Type = type;
		Name = type.ToString();
		OrganVariables = new Dictionary<string, NeuralNetworkCreatureVariable>();
		MutatableVariable = new NeuralNetworkCreatureVariable("MutatableVariable", Random.Range(VariableMinValue, VariableMaxValue), VariableMinValue, VariableMaxValue);

		if (variables != null)
		{
			foreach (NeuralNetworkCreatureVariable v in variables)
			{
				OrganVariables.Add(v.Name, v);
			}
		}
	}

	public void SetCreature(NeuralNetworkCreature creature)
	{
		_creature = creature;
	}

	public virtual void Mutate()
	{
		float m = GameController.Instance.RollMutationFactor();
		MutatableVariable.Value += m;
		MutatableVariable.Value = Mathf.Clamp(MutatableVariable.Value, MutatableVariable.Min, MutatableVariable.Max);
	}

	public virtual NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		NeuralNetworkCreatureOrgan copy = new NeuralNetworkCreatureOrgan();
		copy.Initialize(_creature, Type, OrganVariables.Values.ToList());

		return copy;
	}
}