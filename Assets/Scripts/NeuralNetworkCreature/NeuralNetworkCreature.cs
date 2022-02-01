using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetworkCreature : MonoBehaviour, INeuralNetworkCreature
{
	[Header("Prefab Settings")]
	public List<NeuralNetworkCreatureOrganScriptableObject> InputOrgans;
	public List<NeuralNetworkCreatureOrganScriptableObject> OutputOrgans;
	public List<NeuralNetworkCreatureTraitScriptableObject> InheritableTraits;

	//	Internal variables
	protected bool _initialized = false;
	protected int _internalLayers;
	protected int _internalLayerSize;

	//	Organ dictionaries
	protected Dictionary<string, NeuralNetworkCreatureInputOrgan> _inputOrgans = new Dictionary<string, NeuralNetworkCreatureInputOrgan>();
	protected Dictionary<string, NeuralNetworkCreatureOutputOrgan> _outputOrgans = new Dictionary<string, NeuralNetworkCreatureOutputOrgan>();
	protected Dictionary<string, NeuralNetworkCreatureInheritableTrait> _traits = new Dictionary<string, NeuralNetworkCreatureInheritableTrait>();

	//	Events
	public delegate void CollisionEvent(Collision collision);
	public CollisionEvent OnCreatureCollision;

	//	NeuralNetwork
	public NeuralNetwork Network { get; protected set; }

	/// <summary>
	/// Initialize the creature with a new NeuralNetwork using default settings.
	/// </summary>
	public void Initialize()
	{
		List<NeuralNetworkCreatureInputOrgan> inputOrgans = new List<NeuralNetworkCreatureInputOrgan>();
		List<NeuralNetworkCreatureOutputOrgan> outputOrgans = new List<NeuralNetworkCreatureOutputOrgan>();

		foreach(NeuralNetworkCreatureOrganScriptableObject o in InputOrgans)
		{
			inputOrgans.Add((NeuralNetworkCreatureInputOrgan)o.Instantiate(this));
		}

		foreach(NeuralNetworkCreatureOrganScriptableObject o in OutputOrgans)
		{
			outputOrgans.Add((NeuralNetworkCreatureOutputOrgan)o.Instantiate(this));
		}

		Initialize(inputOrgans, outputOrgans);
	}

	/// <summary>
	/// Initialize the creature with a new NeuralNetwork using the given organs and input settings.
	/// </summary>
	public void Initialize(List<NeuralNetworkCreatureInputOrgan> inputOrgans, List<NeuralNetworkCreatureOutputOrgan> outputOrgans, List<NeuralNetworkCreatureInheritableTrait> traits = null, int internalLayers = 2, int internalLayerSize = 8)
	{
		if (traits != null)
		{
			ApplyTraits(traits);
		}
		else
		{
			ApplyTraits();
		}

		//	Create the Neural Network
		_internalLayers = internalLayers;
		_internalLayerSize = internalLayerSize;
		int[] layers = new int[2 + _internalLayers];	//	1 layer for inputs, 1 layer for outputs + internal layers inbetween
		layers[0] = inputOrgans.Sum(x => x.SensorCount);

		for(int i = 0; i < _internalLayers; i++)
		{
			layers[i + 1] = _internalLayerSize;
		}

		layers[1 + _internalLayers] = outputOrgans.Count;

		Network = new NeuralNetwork(layers);

		//	Initialize creature input organs
		for(int i = 0; i < inputOrgans.Count; i++)
		{
			_inputOrgans.Add(inputOrgans[i].Name, inputOrgans[i]);
		}

		//	Initialize creature output organs
		for (int i = 0; i < outputOrgans.Count; i++)
		{
			_outputOrgans.Add(outputOrgans[i].Name, outputOrgans[i]);
		}

		_initialized = true;
	}

	/// <summary>
	/// Initialize the NeuralNetwork using the given network. (Does not create a copy, create or copy the network first)
	/// </summary>
	public void Initialize(NeuralNetwork network, List<NeuralNetworkCreatureInheritableTrait> traits)
	{
		ApplyTraits(traits);

		Network = network;
		_internalLayers = network.Layers.Length - 2;
		if (_internalLayers > 0)
		{
			_internalLayerSize = network.Layers[1];
		}

		List<NeuralNetworkCreatureInputOrgan> inputOrgans = new List<NeuralNetworkCreatureInputOrgan>();
		List<NeuralNetworkCreatureOutputOrgan> outputOrgans = new List<NeuralNetworkCreatureOutputOrgan>();

		foreach (NeuralNetworkCreatureOrganScriptableObject o in InputOrgans)
		{
			inputOrgans.Add((NeuralNetworkCreatureInputOrgan)o.Instantiate(this));
		}

		foreach (NeuralNetworkCreatureOrganScriptableObject o in OutputOrgans)
		{
			outputOrgans.Add((NeuralNetworkCreatureOutputOrgan)o.Instantiate(this));
		}

		//	Initialize creature input organs
		for (int i = 0; i < inputOrgans.Count; i++)
		{
			_inputOrgans.Add(inputOrgans[i].Name, inputOrgans[i]);
		}

		//	Initialize creauture output organs
		for (int i = 0; i < outputOrgans.Count; i++)
		{
			_outputOrgans.Add(outputOrgans[i].Name, outputOrgans[i]);
		}

		_initialized = true;
	}

	public void ApplyTraits()
	{
		foreach(NeuralNetworkCreatureTraitScriptableObject t in InheritableTraits)
		{
			NeuralNetworkCreatureInheritableTrait trait = t.Instantiate(this);
			trait.MutateTraitValue();
			trait.ApplyTraitValue();
			_traits.Add(trait.Name, trait);
		}
	}

	public void ApplyTraits(List<NeuralNetworkCreatureInheritableTrait> inheritedTraits)
	{
		foreach (NeuralNetworkCreatureInheritableTrait trait in inheritedTraits)
		{
			NeuralNetworkCreatureInheritableTrait newTrait = trait.CreateDeepCopy(this);
			newTrait.MutateTraitValue();
			newTrait.ApplyTraitValue();
			_traits.Add(newTrait.Name, newTrait);
		}
	}

	public List<NeuralNetworkCreatureInheritableTrait> GetTraits()
	{
		return _traits.Values.ToList();
	}

	/// <summary>
	/// Run the Neural Network FeedForward function using inputs from the creature's organs and updating the values of the creatures output emitting organs.
	/// </summary>
	public void ProcessNetworkInputs()
	{
		List<float> inputs = new List<float>();

		foreach (NeuralNetworkCreatureInputOrgan organ in _inputOrgans.Values)
		{
			organ.UpdateSensors();
			float[] organInputs = organ.GetInputValues();

			for(int i = 0; i < organInputs.Length; i++)
			{
				inputs.Add(organInputs[i]);
			}
		}

		float[] outputs = Network.FeedForward(inputs.ToArray());

		if(outputs.Length != _outputOrgans.Count)
		{
			throw new System.Exception("output length does not match organ count");
		}

		int j = 0;
		foreach (NeuralNetworkCreatureOutputOrgan organ in _outputOrgans.Values)
		{
			organ.SetOutputValue(outputs[j]);
			organ.Process();
			j++;
		}
	}

	/// <summary>
	/// Triggers OnCreatureCollision event callback that organs can subscribe to
	/// </summary>
	private void OnCollisionEnter(Collision collision)
	{
		if (OnCreatureCollision != null)
		{
			OnCreatureCollision(collision);
		}
	}

	private void OnDestroy()
	{
		GameObject.Destroy(GetComponent<MeshRenderer>().material);
	}

	/// <summary>
	/// FixedUpdate loop processes the NeuralNetwork every FixedUpdate interval.
	/// </summary>
	private void FixedUpdate()
	{
		if(!_initialized)
		{
			return;
		}

		ProcessNetworkInputs();
	}
}