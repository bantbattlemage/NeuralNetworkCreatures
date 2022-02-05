using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class NeuralNetworkCreature : MonoBehaviour, INeuralNetworkCreature
{
	[Header("Prefab Settings")]
	public List<NeuralNetworkCreatureOrganScriptableObject> InputOrgans;
	public List<NeuralNetworkCreatureOrganScriptableObject> OutputOrgans;
	public List<NeuralNetworkCreatureOrganScriptableObject> InheritableTraits;

	//	Internal variables
	protected bool _initialized = false;
	protected int _internalLayers;
	protected int _internalLayerSize;

	//	Organ dictionaries
	public Dictionary<string, NeuralNetworkCreatureInputOrgan> _inputOrgans = new Dictionary<string, NeuralNetworkCreatureInputOrgan>();
	public Dictionary<string, NeuralNetworkCreatureOutputOrgan> _outputOrgans = new Dictionary<string, NeuralNetworkCreatureOutputOrgan>();
	public Dictionary<string, NeuralNetworkCreatureInheritableTrait> _traits = new Dictionary<string, NeuralNetworkCreatureInheritableTrait>();

	//	Events
	public delegate void CollisionEvent(Collision collision);
	public CollisionEvent OnCreatureCollision;

	//	NeuralNetwork
	public NeuralNetwork Network { get; protected set; }

	public NeuralNetworkCreatureData StoredData;

	//void Start()
	//{
	//	DynamicObject dynamicObject = GetComponent<DynamicObject>();
	//	dynamicObject.prepareToSaveDelegates += PrepareToSaveObjectState;
	//	dynamicObject.loadObjectStateDelegates += LoadObjectState;
	//}

	//private void LoadObjectState(ObjectState objectState)
	//{
	//	StoredData = SaveUtils.FromJson<NeuralNetworkCreatureData>(Convert.ToString(objectState.genericValues["NeuralNetworkCreature.StoredData"]));
	//}

	//private void PrepareToSaveObjectState(ObjectState objectState)
	//{
	//	objectState.genericValues["NeuralNetworkCreature.StoredData"] = SaveUtils.ToJson(GetCreatureData());
	//}

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
	/// Initialize the creature creating exact copies of the given network, organs, and traits
	/// </summary>
	public void Initialize(NeuralNetwork network, List<NeuralNetworkCreatureInputOrgan> inputOrgans, List<NeuralNetworkCreatureOutputOrgan> outputOrgans, List<NeuralNetworkCreatureInheritableTrait> traits)
	{
		Network = network;
		_internalLayers = network.Layers.Length - 2;
		_internalLayerSize = network.Layers.Length - 2 > 0 ? network.Layers[1] : 0;

		_inputOrgans = new Dictionary<string, NeuralNetworkCreatureInputOrgan>();
		//foreach (NeuralNetworkCreatureInputOrgan o in inputOrgans)
		//{
		//	//NeuralNetworkCreatureOrgan copy = o.CreateDeepCopy(this);
		//	o.SetCreature(this);
		//	_inputOrgans.Add(o.Name, o);

		//}

		_outputOrgans = new Dictionary<string, NeuralNetworkCreatureOutputOrgan>();
		//foreach (NeuralNetworkCreatureOutputOrgan o in outputOrgans)
		//{
		//	//NeuralNetworkCreatureOrgan copy = o.CreateDeepCopy(this);
		//	o.SetCreature(this);
		//	_outputOrgans.Add(o.Name, o);
		//}

		//_traits = new Dictionary<string, NeuralNetworkCreatureInheritableTrait>();
		//foreach (NeuralNetworkCreatureInheritableTrait o in traits)
		//{
		//	NeuralNetworkCreatureInheritableTrait copy = o.CreateDeepCopy(this) as NeuralNetworkCreatureInheritableTrait;
		//	copy.ApplyTraitValue();
		//	_traits.Add(copy.Name, copy);
		//}

		InitializeInputOrgans(inputOrgans);
		InitializeOutputOrgans(outputOrgans);
		InitializeTraits(traits);

		_initialized = true;
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
		layers[0] = inputOrgans.Sum(x => x.OrganVariables.Count);

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
			_inputOrgans.Add(inputOrgans[i].Name, inputOrgans[i]);
		}

		//	Initialize creauture output organs
		for (int i = 0; i < outputOrgans.Count; i++)
		{
			outputOrgans[i].MutatableVariable = parent._outputOrgans[outputOrgans[i].Name].MutatableVariable.Copy();	//	set the internal organ modifier value to the parent's value
			outputOrgans[i].Mutate();
			_outputOrgans.Add(outputOrgans[i].Name, outputOrgans[i]);
		}

		_initialized = true;
	}

	/// <summary>
	/// Returns deep copied creature data structure.
	/// </summary>
	public NeuralNetworkCreatureData GetCreatureData()
	{
		NeuralNetworkCreatureData data = new NeuralNetworkCreatureData
		{
			Network = Network,
			InputOrgans = _inputOrgans,
			OutputOrgans = _outputOrgans,
			Traits = _traits
		};

		return data;
	}

	/// <summary>
	/// 
	/// </summary>
	public void SaveToJson(NeuralNetworkCreatureData data = null)
	{
		if(data == null)
		{
			data = GetCreatureData();
		}

		string json = SaveUtils.ToJson(data);
		string filePath = Application.persistentDataPath + "/newSavedCreature.json";

		if (File.Exists(filePath))
		{
			int count = 0;
			while (File.Exists(filePath))
			{
				filePath = Application.persistentDataPath + string.Format("/newSavedCreature{0}.json", count);
				count++;
			}

			using FileStream fs = File.Create(filePath);
			fs.Close();
			File.WriteAllText(filePath, json);
		}
		else
		{
			using FileStream fs = File.Create(filePath);
			fs.Close();
			File.WriteAllText(filePath, json);
		}
	}

	public void LoadFromJson(string filePath)
	{
		string json;

		if (File.Exists(filePath))
		{
			json = File.ReadAllText(filePath);
		}
		else
		{
			throw new Exception("Could not find " + filePath);
		}

		NeuralNetworkCreatureData data = SaveUtils.FromJson<NeuralNetworkCreatureData>(json);
		//SaveToJson(data);

		Initialize(data.Network, data.InputOrgans.Values.ToList(), data.OutputOrgans.Values.ToList(), data.Traits.Values.ToList());
	}

	/// <summary>
	/// Initialize this creature's InheritableTraits using copies of the given parent traits, mutates them, and then applies the traits.
	/// </summary>
	public void InitializeTraits(List<NeuralNetworkCreatureInheritableTrait> inheritedTraits, bool mutate = true)
	{
		foreach (NeuralNetworkCreatureInheritableTrait trait in inheritedTraits)
		{
			NeuralNetworkCreatureInheritableTrait newTrait = null;

			switch (trait.Type)
			{
				case NeuralNetworkCreatureOrganType.ColorTrait:
					ColorTrait c = new ColorTrait();
					c.Initialize(this, trait.Type, trait.OrganVariables.Values.ToList());
					c.MutatableVariable = trait.MutatableVariable;
					c.ApplyTraitValue();
					newTrait = c;
					break;
				default:
					//newTrait = trait.CreateDeepCopy(this) as NeuralNetworkCreatureInheritableTrait;
					throw new Exception("not a trait!");
			}

			if(mutate)
			{
				newTrait.Mutate();
			}

			_traits.Add(newTrait.Name, newTrait);
		}
	}

	public void InitializeInputOrgans(List<NeuralNetworkCreatureInputOrgan> inputOrgans, bool mutate = true)
	{
		foreach (NeuralNetworkCreatureInputOrgan organ in inputOrgans)
		{
			NeuralNetworkCreatureInputOrgan newOrgan = null;
			List<NeuralNetworkCreatureVariable> variables = organ.OrganVariables.Values.ToList();

			switch (organ.Type)
			{
				case NeuralNetworkCreatureOrganType.Heartbeat:
					newOrgan = new HeartbeatInputOrgan();
					break;
				case NeuralNetworkCreatureOrganType.SpatialAwareness:
					newOrgan = new SpatialAwarenessInputOrgan();
					break;
				case NeuralNetworkCreatureOrganType.BasicVision:
					newOrgan = new BasicVisionInputOrgan();
					break;
				case NeuralNetworkCreatureOrganType.BasicPelletConsumption:
					newOrgan = new BasicPelletConsumptionInputOrgan();
					break;
				default:
					throw new Exception("not an inputorgan!");
			}

			newOrgan.Initialize(this, organ.Type, variables);
			newOrgan.MutatableVariable.Value = organ.MutatableVariable.Value;

			if (mutate)
			{
				newOrgan.Mutate();
			}

			_inputOrgans.Add(newOrgan.Name, newOrgan);
		}
	}

	public void InitializeOutputOrgans(List<NeuralNetworkCreatureOutputOrgan> outputOrgans, bool mutate = true)
	{
		foreach (NeuralNetworkCreatureOutputOrgan organ in outputOrgans)
		{
			NeuralNetworkCreatureOutputOrgan newOrgan = null;
			List<NeuralNetworkCreatureVariable> variables = organ.OrganVariables.Values.ToList();

			switch (organ.Type)
			{
				case NeuralNetworkCreatureOrganType.BasicMovement:
					newOrgan = new BasicMovementOutputOrgan();
					break;
				case NeuralNetworkCreatureOrganType.BasicRotation:
					newOrgan = new BasicRotationOutputOrgan();
					break;
				default:
					throw new Exception("not an inputorgan!");
			}

			newOrgan.Initialize(this, organ.Type, variables);
			newOrgan.MutatableVariable.Value = organ.MutatableVariable.Value;

			if (mutate)
			{
				newOrgan.Mutate();
			}

			_outputOrgans.Add(newOrgan.Name, newOrgan);
		}
	}

	/// <summary>
	/// Apply all trait values.
	/// </summary>
	public void ApplyTraits()
	{
		foreach (NeuralNetworkCreatureOrganScriptableObject t in InheritableTraits)
		{
			NeuralNetworkCreatureInheritableTrait trait = t.Instantiate(this, true);
			trait.Mutate();
			trait.ApplyTraitValue();
			_traits.Add(trait.Name, trait);
		}
	}

	/// <summary>
	/// Returns a list of InheritableTraits.
	/// </summary>
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

		if(outputs.Length == 0 || _outputOrgans.Count == 0)
		{
			throw new Exception("no output organs!");
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
	/// mutationChance/100 chance per value to mutate NeuralNetwork and Organ values.
	/// </summary>
	public void Mutate(int mutationChance, float mutationStrength)
	{
		Network.Mutate(mutationChance, mutationStrength);

		foreach (NeuralNetworkCreatureOutputOrgan o in _outputOrgans.Values)
		{
			o.Mutate();
		}
	}

	/// <summary>
	/// Returns a new NeuralNetworkCreature object with traits mixed from the given otherParent and this creature, and instantiates a GameObject for it at the given location.
	/// </summary>
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

	/// <summary>
	/// OnDestroy
	/// </summary>
	private void OnDestroy()
	{
		Destroy(GetComponent<MeshRenderer>().material);
	}

	/// <summary>
	/// FixedUpdate loop processes the NeuralNetwork every FixedUpdate interval.
	/// </summary>
	private void FixedUpdate()
	{
		if (!_initialized)
		{
			return;
		}

		ProcessNetworkInputs();
	}
}