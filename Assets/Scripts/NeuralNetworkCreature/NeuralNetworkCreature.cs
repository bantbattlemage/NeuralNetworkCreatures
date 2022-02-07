using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class NeuralNetworkCreature : MonoBehaviour, INeuralNetworkCreature
{
	[Header("Prefab Settings")]
	public List<NeuralNetworkCreatureOrganScriptableObject> InputOrgansPrefabs;
	public List<NeuralNetworkCreatureOrganScriptableObject> OutputOrgansPrefabs;
	public List<NeuralNetworkCreatureOrganScriptableObject> InheritableTraitPrefabs;

	//	Internal variables
	protected bool _initialized = false;
	protected int _internalLayers;
	protected int _internalLayerSize;

	//	Organ dictionaries
	public Dictionary<string, NeuralNetworkCreatureInputOrgan> InputOrgans = new Dictionary<string, NeuralNetworkCreatureInputOrgan>();
	public Dictionary<string, NeuralNetworkCreatureOutputOrgan> OutputOrgans = new Dictionary<string, NeuralNetworkCreatureOutputOrgan>();
	public Dictionary<string, NeuralNetworkCreatureInheritableTrait> Traits = new Dictionary<string, NeuralNetworkCreatureInheritableTrait>();

	//	Events
	public delegate void CollisionEvent(Collision collision);
	public event CollisionEvent OnCreatureCollision;
	//public delegate void CreatureDeathEvent(NeuralNetworkCreature creature);
	//public event CreatureDeathEvent CreatureDiedEvent;

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

	void Start()
	{
		GameController.Instance.World.SimulationUpdate += OnSimulationOnUpdate;
	}

	private void OnSimulationOnUpdate()
	{
		if(!_initialized)
		{
			return;
		}

		ProcessNetworkInputs();
	}

	/// <summary>
	/// Initialize the creature with a new NeuralNetwork using default settings.
	/// </summary>
	public void Initialize()
	{
		List<NeuralNetworkCreatureInputOrgan> inputOrgans = new List<NeuralNetworkCreatureInputOrgan>();
		List<NeuralNetworkCreatureOutputOrgan> outputOrgans = new List<NeuralNetworkCreatureOutputOrgan>();

		foreach(NeuralNetworkCreatureOrganScriptableObject o in InputOrgansPrefabs)
		{
			inputOrgans.Add((NeuralNetworkCreatureInputOrgan)o.Instantiate(this));
		}

		foreach(NeuralNetworkCreatureOrganScriptableObject o in OutputOrgansPrefabs)
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

		InputOrgans = new Dictionary<string, NeuralNetworkCreatureInputOrgan>();
		OutputOrgans = new Dictionary<string, NeuralNetworkCreatureOutputOrgan>();
	
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
			InputOrgans.Add(inputOrgans[i].Name, inputOrgans[i]);
		}

		//	Initialize creature output organs
		for (int i = 0; i < outputOrgans.Count; i++)
		{
			OutputOrgans.Add(outputOrgans[i].Name, outputOrgans[i]);
		}

		_initialized = true;
	}

	/// <summary>
	/// Initialize the NeuralNetwork with a given parent and inherit values from it. (Does not create a copy, create a new network first then call this to initialize it)
	/// </summary>
	public void Initialize(NeuralNetwork network, NeuralNetworkCreature parent)
	{
		InitializeTraits(parent.GetTraits());
		InitializeInputOrgans(parent.GetInputOrgans());
		InitializeOutputOrgans(parent.GetOutputOrgans());

		Network = network;
		_internalLayers = network.Layers.Length - 2;
		if (_internalLayers > 0)
		{
			_internalLayerSize = network.Layers[1];
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
			InputOrgans = InputOrgans,
			OutputOrgans = OutputOrgans,
			Traits = Traits
		};

		return data;
	}

	public List<NeuralNetworkCreatureInheritableTrait> GetTraits()
	{
		return Traits.Values.ToList();
	}

	public List<NeuralNetworkCreatureInputOrgan> GetInputOrgans()
	{
		return InputOrgans.Values.ToList();
	}

	public List<NeuralNetworkCreatureOutputOrgan> GetOutputOrgans()
	{
		return OutputOrgans.Values.ToList();
	}

	public void SaveToJson(NeuralNetworkCreatureData data = null)
	{
		if(data == null)
		{
			data = GetCreatureData();
		}

		string json = SaveUtils.ToJson(data);
		string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "SavedData" + Path.DirectorySeparatorChar + "savedCreature0.json";

		if(!Directory.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "SavedData"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "SavedData");
		}

		if (File.Exists(filePath))
		{
			int count = 0;
			while (File.Exists(filePath))
			{
				filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "SavedData" + Path.DirectorySeparatorChar + string.Format("savedCreature{0}.json", count);
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
					c.MutatableVariable = trait.MutatableVariable.Copy();
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

			Traits.Add(newTrait.Name, newTrait);
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
				case NeuralNetworkCreatureOrganType.Photosynthesis:
					newOrgan = new PhotosynthesisInputOrgan();
					break;
				case NeuralNetworkCreatureOrganType.EnergyStorage:
					newOrgan = new EnergyStorageInputOrgan();
					break;
				case NeuralNetworkCreatureOrganType.Roots:
					newOrgan = new RootsOrgan();
					break;
				default:
					throw new Exception("not an inputorgan!");
			}

			newOrgan.Initialize(this, organ.Type);
			newOrgan.MutatableVariable = organ.MutatableVariable.Copy();

			if (mutate)
			{
				newOrgan.Mutate();
			}

			InputOrgans.Add(newOrgan.Name, newOrgan);
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
				case NeuralNetworkCreatureOrganType.PlantReproduction:
					newOrgan = new PlantReproductionOutputOrgan();
					break;
				case NeuralNetworkCreatureOrganType.PlantGrowth:
					newOrgan = new PlantGrowthOutputOrgan();
					break;
				default:
					throw new Exception("not an output organ!");
			}

			newOrgan.Initialize(this, organ.Type, variables);
			newOrgan.MutatableVariable = organ.MutatableVariable.Copy();

			if (mutate)
			{
				newOrgan.Mutate();
			}

			OutputOrgans.Add(newOrgan.Name, newOrgan);
		}
	}

	/// <summary>
	/// Apply all trait values.
	/// </summary>
	public void ApplyTraits()
	{
		foreach (NeuralNetworkCreatureOrganScriptableObject t in InheritableTraitPrefabs)
		{
			NeuralNetworkCreatureInheritableTrait trait = t.Instantiate(this, true);
			trait.Mutate();
			trait.ApplyTraitValue();
			Traits.Add(trait.Name, trait);
		}
	}

	/// <summary>
	/// Run the Neural Network FeedForward function using inputs from the creature's organs and updating the values of the creatures output emitting organs.
	/// </summary>
	public void ProcessNetworkInputs()
	{
		List<float> inputs = new List<float>();

		foreach (NeuralNetworkCreatureInputOrgan organ in InputOrgans.Values)
		{
			organ.UpdateSensors();
			float[] organInputs = organ.GetInputValues();

			for(int i = 0; i < organInputs.Length; i++)
			{
				inputs.Add(organInputs[i]);
			}
		}

		float[] outputs = Network.FeedForward(inputs.ToArray());

		if(outputs.Length != OutputOrgans.Count)
		{
			throw new System.Exception("output length does not match organ count");
		}

		if(outputs.Length == 0 || OutputOrgans.Count == 0)
		{
			throw new Exception("no output organs!");
		}

		int j = 0;
		foreach (NeuralNetworkCreatureOutputOrgan organ in OutputOrgans.Values)
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

		foreach (NeuralNetworkCreatureOutputOrgan o in OutputOrgans.Values)
		{
			o.Mutate();
		}
	}

	/// <summary>
	/// Returns a new NeuralNetworkCreature object with traits mixed from the given otherParent and this creature, and instantiates a GameObject for it at the given location.
	/// </summary>
	public NeuralNetworkCreature Reproduce(NeuralNetworkCreature otherParent, Vector3 location)
	{
		Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
		NeuralNetworkCreature newCreature = Instantiate(gameObject, location, rotation).GetComponent<NeuralNetworkCreature>();
		newCreature.Initialize(Network.CreateChildNetwork(otherParent.Network), this);
		newCreature.Mutate(GameController.Instance.MutationChance, GameController.Instance.MutationStrength);
		newCreature.gameObject.name = "NewCreature";
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
		if(GameController.Instance.Quitting)
		{ 
			return; 
		}

		GameController.Instance.World.SimulationUpdate -= OnSimulationOnUpdate;

		MeshRenderer[] children = transform.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in children)
		{
			Destroy(r.material);
		}

		SpatialAwarenessInputOrgan locationOrgan = InputOrgans["SpatialAwareness"] as SpatialAwarenessInputOrgan;
		GameController.Instance.World.GetWorldTile(locationOrgan.GetWorldCoordinatesInt()).AddSoilEnergy(1f);
		//Debug.Log(locationOrgan.GetWorldCoordinatesInt() + " " + GameController.Instance.World.GetWorldTile(locationOrgan.GetWorldCoordinatesInt()).GetSoilQuality());
	}

	/// <summary>
	/// FixedUpdate loop processes the NeuralNetwork every FixedUpdate interval.
	/// </summary>
	//private void FixedUpdate()
	//{
	//	if (!_initialized)
	//	{
	//		return;
	//	}

	//	ProcessNetworkInputs();
	//}
}