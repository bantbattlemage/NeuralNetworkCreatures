using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[Header("Simulation Settings")]
	public float Timescale;
	public float GenerationDuration;
	public Vector2 WorldSize;
	public int Population;
	public int MutationChance;
	public float MutationStrength;

	[Header("Project References")]
	public GameObject NeuralNetworkCreaturePrefab;
	public GameObject PelletPrefab;

	[Header("Scene References")]
	public Text UIText;
	public TimeScaleController TimeScaler;
	public GameObject WorldFloor;

	private int _currentGeneration;
	private List<NeuralNetworkCreature> _creatures = new List<NeuralNetworkCreature>();
	private List<PelletObject> _pellets = new List<PelletObject>();

	public static GameController Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		InitializeWorld();
		InvokeRepeating("IncrementSimulation", GenerationDuration, GenerationDuration);
		TimeScaler.SetTimeScale(Timescale);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			NeuralNetworkCreature best = _creatures.OrderByDescending(x => x.Network.Fitness).First();
			best.SaveToJson();
		}
	}

	public void LoadCreature(string filePath)
	{
		CancelInvoke();
		DestroyPellets();

		foreach (NeuralNetworkCreature old in _creatures)
		{
			Destroy(old);
			Destroy(old.gameObject);
		}

		_creatures = new List<NeuralNetworkCreature>();

		for (int i = 0; i < Population; i++)
		{
			Vector3 randomCoords = GetRandomWorldCoordinates(5);
			NeuralNetworkCreature newCreature = Instantiate(NeuralNetworkCreaturePrefab, randomCoords, new Quaternion()).GetComponent<NeuralNetworkCreature>();
			newCreature.LoadFromJson(filePath);
			newCreature.Mutate(MutationChance, MutationStrength);
			_creatures.Add(newCreature);
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
			Vector3 randomCoords = GetRandomWorldCoordinates(3, 3);
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

	private void IncrementSimulation()
	{
		int remainingPellets = DestroyPellets();

		List<NeuralNetworkCreature> sortedObjects = _creatures.OrderByDescending(x => x.Network.Fitness).ToList();

		float average = 0;
		float total = 0;
		for(int i = 0; i < sortedObjects.Count; i++)
		{
			total += sortedObjects[i].Network.Fitness;
		}
		average = total / sortedObjects.Count;

		string summary = string.Format("Generation {0}: \nBest: {1}\nWorst: {2}\nAverage: {3}\nRemaining Pellets: {4}", _currentGeneration, sortedObjects.First().Network.Fitness, sortedObjects.Last().Network.Fitness, average, ((float)remainingPellets/(float)Population).ToString("00.00%"));
		//Debug.Log(summary);
		UIText.text = summary;

		_currentGeneration++;
		List<NeuralNetworkCreature> newGeneration = new List<NeuralNetworkCreature>();

		while (newGeneration.Count < Population)
		{
			for (int i = 0; i < sortedObjects.Count/2; i++)
			{
				Vector3 randomCoords = GetRandomWorldCoordinates(5, 5);

				NeuralNetworkCreature father = sortedObjects[Random.Range(0, i)];
				NeuralNetworkCreature newCreature = sortedObjects[i].Reproduce(father, randomCoords);
				newGeneration.Add(newCreature);

				if(newGeneration.Count >= Population)
				{
					break;
				}
			}
		}

		foreach(NeuralNetworkCreature old in _creatures)
		{
			Destroy(old);
			Destroy(old.gameObject);
		}

		_creatures = new List<NeuralNetworkCreature>();
		_creatures = newGeneration;

		SpawnPellets();
	}

	public void InitializeWorld()
	{
		WorldFloor.transform.localScale = new Vector3(WorldSize.x / 10f, 1, WorldSize.y / 10f);

		_creatures = new List<NeuralNetworkCreature>();
		_currentGeneration = 0;

		for (int i = 0; i < Population; i++)
		{
			Vector3 randomCoords = new Vector3(Random.Range(0, WorldSize.x - 5) - WorldSize.x / 2, 0, Random.Range(0, WorldSize.y - 5) - WorldSize.y / 2);

			NeuralNetworkCreature newCreature = Instantiate(NeuralNetworkCreaturePrefab, randomCoords, new Quaternion()).GetComponent<NeuralNetworkCreature>();
			newCreature.Initialize();
			newCreature.Network.Fitness = 0;

			_creatures.Add(newCreature);
		}

		SpawnPellets();
	}

	public bool IsOutOfBounds(Vector3 vector)
	{
		return vector.x >= WorldSize.x / 2 || vector.x <= -(WorldSize.x / 2) || vector.z >= WorldSize.y / 2 || vector.z <= -(WorldSize.y / 2);
	}

	public Vector3 GetRandomWorldCoordinates(int xOffset = 0, int yOffset = 0)
	{
		return new Vector3(Random.Range(0, WorldSize.x - xOffset) - WorldSize.x / 2, 0, Random.Range(0, WorldSize.y - yOffset) - WorldSize.y / 2);
	}

	public float RollMutationFactor()
	{
		if (Random.Range(0, 100) < MutationChance)
		{
			return Random.Range(-MutationStrength, MutationStrength);
		}

		return 0;
	}
}