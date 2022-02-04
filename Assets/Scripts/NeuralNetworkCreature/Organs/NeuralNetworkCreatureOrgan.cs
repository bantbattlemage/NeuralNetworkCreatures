using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkCreatureOrgan : INeuralNetworkCreatureOrgan
{
	public string Name { get; protected set; }
	public virtual NeuralNetworkCreatureOrganType Type { get; protected set; }
	public NeuralNetworkCreatureVariable MutatableVariable { get; set; }
	public Dictionary<string, NeuralNetworkCreatureVariable> OrganVariables { get; protected set; }

	protected NeuralNetworkCreature _creature;

	public virtual void Initialize(NeuralNetworkCreature creature, NeuralNetworkCreatureOrganType type, List<NeuralNetworkCreatureVariable> variables = null)
	{
		_creature = creature;
		Type = type;
		Name = type.ToString();
		OrganVariables = new Dictionary<string, NeuralNetworkCreatureVariable>();
		MutatableVariable = new NeuralNetworkCreatureVariable("MutatableVariable", Random.Range(-1, 1));

		if (variables != null)
		{
			foreach (NeuralNetworkCreatureVariable v in variables)
			{
				OrganVariables.Add(v.Name, v);
			}
		}
	}

	public virtual void Mutate()
	{
		float m = GameController.Instance.RollMutationFactor();
		MutatableVariable += m;
		MutatableVariable.Value = Mathf.Clamp(MutatableVariable.Value, MutatableVariable.Min, MutatableVariable.Max);
	}

	public virtual NeuralNetworkCreatureOrgan CreateDeepCopy()
	{
		throw new System.NotImplementedException();
	}
}