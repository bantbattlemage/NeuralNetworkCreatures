using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationWorld : MonoBehaviour
{
	//public GameObject WorldFloor;
	public Transform WorldOrigin;
	public Transform CenterPoint;
	public GameObject WorldTilePrefab;

	public float SimulationInterval;

	public WorldTile[,] WorldTiles;

	public int SimulationTick { get; private set; }

	public delegate void SimulationUpdateEvent();
	public event SimulationUpdateEvent SimulationUpdate;

	public void InitializeWorld(int worldSize)
	{
		SimulationTick = 0;
		WorldOrigin.transform.position = new Vector3(-worldSize/2f, 0, -worldSize/2f);
		CenterPoint.transform.localPosition = new Vector3(worldSize/2f, 0, worldSize/2f);
		WorldTiles = new WorldTile[worldSize, worldSize];

		for(int i = 0; i < worldSize; i++)
		{
			for(int j = 0; j < worldSize; j++)
			{
				WorldTile newTile = Instantiate(WorldTilePrefab, WorldOrigin).GetComponent<WorldTile>();
				newTile.Initialize(new Vector2Int(i, j));
				WorldTiles[i, j] = newTile;
			}
		}
	}

	public Vector3 GetWorldCenterPoint()
	{
		return new Vector3(WorldTiles.Length / 2f, 0, WorldTiles.Length / 2f);
	}

	public void StartSimulation()
	{
		InvokeRepeating("UpdateSimulation", SimulationInterval, SimulationInterval);
	}

	public void PauseSimulation()
	{
		CancelInvoke();
	}

	public void StopSimulation()
	{

	}

	private void UpdateSimulation()
	{
		if(SimulationUpdate != null)
		{
			SimulationUpdate?.Invoke();
		}

		SimulationTick++;
		GameController.Instance.UpdateUIText();
	}

	public float GetSunlightValue(Vector2Int location)
	{
		return 1;
	}

	public SoilProperties GetSoilProperties(Vector2Int location)
	{
		return WorldTiles[location.x, location.y].Soil;
	}
}