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
			InitializeTraits(traits);
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
			_inputOrgans.Add(inputOrgans[i].GetName(), inputOrgans[i]);
		}

		//	Initialize creature output organs
		for (int i = 0; i < outputOrgans.Count; i++)
		{
			_outputOrgans.Add(outputOrgans[i].GetName(), outputOrgans[i]);
		}

		_initialized = true;
	}

	/// <summary>
	/// Initialize the NeuralNetwork with a given parent and inherit values from it. (Does not create a copy, create a new network first then call this to initialize it)
	/// </summary>
	public void Initialize(NeuralNetwork network, NeuralNetworkCreature parent)
	{
		InitializeTraits(parent.GetTraits());

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
			_inputOrgans.Add(inputOrgans[i].GetName(), inputOrgans[i]);
		}

		//	Initialize creauture output organs
		for (int i = 0; i < outputOrgans.Count; i++)
		{
			outputOrgans[i].SetValue(parent._outputOrgans[outputOrgans[i].GetName()].GetValue());	//	set the internal organ modifier value to the parent's value
			outputOrgans[i].Mutate();
			_outputOrgans.Add(outputOrgans[i].GetName(), outputOrgans[i]);
		}

		_initialized = true;
	}

	public void ApplyTraits()
	{
		foreach(NeuralNetworkCreatureTraitScriptableObject t in InheritableTraits)
		{
			NeuralNetworkCreatureInheritableTrait trait = t.Instantiate(this);
			trait.Mutate();
			trait.ApplyTraitValue();
			_traits.Add(trait.GetName(), trait);
		}
	}

	public void InitializeTraits(List<NeuralNetworkCreatureInheritableTrait> inheritedTraits)
	{
		foreach (NeuralNetworkCreatureInheritableTrait trait in inheritedTraits)
		{
			NeuralNetworkCreatureInheritableTrait newTrait = trait.CreateDeepCopy(this);
			newTrait.Mutate();
			newTrait.ApplyTraitValue();
			_traits.Add(newTrait.GetName(), newTrait);
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

	public void Mutate(int mutationChance, float mutationStrength)
	{
		Network.Mutate(mutationChance, mutationStrength);

		foreach (NeuralNetworkCreatureOutputOrgan o in _outputOrgans.Values)
		{
			o.Mutate();
		}
	}

	public NeuralNetworkCreature Reproduce(NeuralNetworkCreature otherParent, Vector3 location)
	{
		NeuralNetworkCreature newCreature = Instantiate(gameObject, location, new Quaternion()).GetComponent<NeuralNetworkCreature>();
		newCreature.Initialize(Network.CreateChildNetwork(otherParent.Network), this);
		newCreature.Mutate(GameController.Instance.MutationChance, GameController.Instance.MutationStrength);
		return newCreature;
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
		Destroy(GetComponent<MeshRenderer>().material);
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