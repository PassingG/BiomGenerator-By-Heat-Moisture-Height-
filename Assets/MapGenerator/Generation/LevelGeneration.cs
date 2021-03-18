using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	[SerializeField]
	private int mapWidthInTiles, mapHeightInTiles;

	[SerializeField]
	private GameObject tilePrefab;

	[SerializeField]
	private int gridSize;

	[SerializeField]
	[Range(0f,2f)]
	private float heatMapPos;

	[SerializeField]
	private LevelData levelData;

	[SerializeField]
	private TreeGeneration treeGeneration;

	void Start()
	{
		GenerateMap();
	}

	void GenerateMap()
	{
		// get the tile dimensions from the tile Prefab
		Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
		int tileWidth = (int)tileSize.x;
		int tileHeight = (int)tileSize.y;

		levelData = new LevelData(gridSize, mapWidthInTiles, mapHeightInTiles);

		float distanceBetweenGrid = 1f / ((float)gridSize);

		// for each Tile, instantiate a Tile in the correct position
		for (int yTileIndex = 0;yTileIndex < mapHeightInTiles; yTileIndex++)
		{
			for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
			{
				// calculate the tile position based on the X and Z indices
				Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
					this.gameObject.transform.position.y + yTileIndex * tileHeight,
					this.gameObject.transform.position.z);
				// instantiate a new Tile
				GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;

				TileData tileData = tile.GetComponent<TileGeneration>().GenerateTile(gridSize, xTileIndex, yTileIndex, heatMapPos, mapHeightInTiles);
				levelData.AddTileData(tileData, xTileIndex, yTileIndex);
			}
		}

		

		treeGeneration.GenerateTrees(this.gridSize * mapWidthInTiles, this.gridSize * mapHeightInTiles, distanceBetweenGrid, levelData);
	}
}
