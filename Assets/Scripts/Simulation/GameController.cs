using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[Header("Simulation Settings")]
	public float Timescale;
	public float GenerationDuration;
	public int WorldSize;
	public int Population;
	public int MutationChance;
	public float MutationStrength;

	[Header("Project References")]
	public GameObject NeuralNetworkCreaturePrefab;
	public GameObject PelletPrefab;

	[Header("Scene UI References")]
	public CameraController GameCamera;
	public Text UIText;
	public TimeScaleController TimeScaleUI;
	public LoadCreatureUI SaveLoadUI;
	public MutationAdjustmentController MutationUI;

	[Header("Scene References")]
	public SimulationWorld World;

	private int _currentGeneration;
	private List<NeuralNetworkCreature> _creatures = new List<NeuralNetworkCreature>();
	private List<PelletObject> _pellets = new List<PelletObject>();

	public static GameController Instance;
	public bool Quitting = false;


	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		InitializeGame();
	}

	private void OnApplicationQuit()
	{
		Quitting = true;
	}

	public void InitializeGame()
	{
		TimeScaleUI.SetTimeScale(Timescale);
		World.InitializeWorld(WorldSize);
		GameCamera.InitializeCamera();

		_creatures = new List<NeuralNetworkCreature>();
		_currentGeneration = 0;

		for (int i = 0; i < Population; i++)
		{
			Vector3 randomCoords = GetRandomWorldCoordinates();
			Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

			NeuralNetworkCreature newCreature = Instantiate(NeuralNetworkCreaturePrefab, randomCoords, randomRotation).GetComponent<NeuralNetworkCreature>();
			newCreature.Initialize();
			newCreature.Network.Fitness = 0;

			AddCreature(newCreature);
		}

		World.StartSimulation();
	}

	public void ResetSimulation()
	{
		World.StopSimulation();
		KillAllCreatures();
		InitializeGame();
	}

	public void SaveBestCreature()
	{
		NeuralNetworkCreature best = _creatures.OrderByDescending(x => x.Network.Fitness).First();
		best.SaveToJson();
	}

	public void LoadCreature(string filePath)
	{
		CancelInvoke();
		DestroyPellets();
		KillAllCreatures();

		for (int i = 0; i < Population; i++)
		{
			Vector3 randomCoords = GetRandomWorldCoordinates();
			NeuralNetworkCreature newCreature = Instantiate(NeuralNetworkCreaturePrefab, randomCoords, new Quaternion()).GetComponent<NeuralNetworkCreature>();
			newCreature.LoadFromJson(filePath);
			newCreature.Mutate(MutationChance, MutationStrength);
			AddCreature(newCreature);
		}

		_currentGeneration = 0;

		SpawnPellets();

		InvokeRepeating("IncrementSimulation", GenerationDuration, GenerationDuration);
	}

	private void SpawnPellets()
	{
		_pellets = new List<PelletObject>();

		for (int i = 0; i < Population; i++)
		{
			Vector3 randomCoords = GetRandomWorldCoordinates();
			PelletObject newObject = Instantiate(PelletPrefab, randomCoords, new Quaternion()).GetComponent<PelletObject>();
			_pellets.Add(newObject);
		}
	}

	private int DestroyPellets()
	{
		int remainingPellets = 0;
		for (int i = 0; i < _pellets.Count; i++)
		{
			if(_pellets[i] != null)
			{
				Destroy(_pellets[i].gameObject);
				remainingPellets++;
			}
		}

		_pellets = new List<PelletObject>();

		return remainingPellets;
	}

	public void AddCreature(NeuralNetworkCreature c)
	{
		_creatures.Add(c);
		//c.CreatureDiedEvent += OnCreatureDeath;
	}

	public NeuralNetworkCreature GetRandomCreature()
	{
		return _creatures[Random.Range(0, _creatures.Count - 1)];
	}

	public void KillCreature(NeuralNetworkCreature creature)
	{
		_creatures.Remove(creature);
		Destroy(creature);
		Destroy(creature.gameObject);
	}

	public void KillAllCreatures()
	{
		foreach (NeuralNetworkCreature old in _creatures)
		{
			Destroy(old);
			Destroy(old.gameObject);
		}
		_creatures = new List<NeuralNetworkCreature>();
	}

	private void OnCreatureDeath(NeuralNetworkCreature creature)
	{

	}

	public bool IsOutOfBounds(Vector3 vector)
	{
		//return vector.x >= WorldSize / 2 || vector.x <= -(WorldSize / 2) || vector.z >= WorldSize / 2 || vector.z <= -(WorldSize / 2);
		return vector.x > WorldSize || vector.x < 0 || vector.z > WorldSize || vector.z < 0;
	}

	public Vector3 GetRandomWorldCoordinates(int xOffset = 0, int yOffset = 0)
	{
		//return new Vector3(Random.Range(0, WorldSize - xOffset) - WorldSize / 2, 0, Random.Range(0, WorldSize - yOffset) - WorldSize / 2);
		return new Vector3(Random.Range(0, WorldSize - xOffset), 0, Random.Range(0, WorldSize - yOffset));
	}

	public float RollMutationFactor()
	{
		if (Random.Range(0, 100) < MutationChance)
		{
			return Random.Range(-MutationStrength, MutationStrength);
		}

		return 0;
	}

	public void UpdateUIText()
	{
		string summary = string.Format("Tick: {0}\nCreatures: {1}", World.SimulationTick, _creatures.Count);
		//Debug.Log(summary);
		UIText.text = summary;
	}

	//private void IncrementSimulation()
	//{
	//	int remainingPellets = DestroyPellets();

	//	List<NeuralNetworkCreature> sortedObjects = _creatures.OrderByDescending(x => x.Network.Fitness).ToList();

	//	float average = 0;
	//	float total = 0;
	//	for (int i = 0; i < sortedObjects.Count; i++)
	//	{
	//		total += sortedObjects[i].Network.Fitness;
	//	}
	//	average = total / sortedObjects.Count;

	//	string summary = string.Format("Generation {0}: \nBest: {1}\nWorst: {2}\nAverage: {3}\nRemaining Pellets: {4}", _currentGeneration, sortedObjects.First().Network.Fitness, sortedObjects.Last().Network.Fitness, average, ((float)remainingPellets / (float)Population).ToString("00.00%"));
	//	//Debug.Log(summary);
	//	UIText.text = summary;

	//	_currentGeneration++;
	//	List<NeuralNetworkCreature> newGeneration = new List<NeuralNetworkCreature>();

	//	while (newGeneration.Count < Population)
	//	{
	//		for (int i = 0; i < sortedObjects.Count / 2; i++)
	//		{
	//			Vector3 randomCoords = GetRandomWorldCoordinates(5, 5);

	//			NeuralNetworkCreature father = sortedObjects[Random.Range(0, i)];
	//			NeuralNetworkCreature newCreature = sortedObjects[i].Reproduce(father, randomCoords);
	//			newGeneration.Add(newCreature);

	//			if (newGeneration.Count >= Population)
	//			{
	//				break;
	//			}
	//		}
	//	}

	//	KillAllCreatures();
	//	_creatures = newGeneration;

	//	SpawnPellets();
	//}
}