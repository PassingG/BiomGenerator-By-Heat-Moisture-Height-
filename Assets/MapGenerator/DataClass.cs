using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
	public float[,] heightMap;
	public float[,] heatMap;
	public float[,] moistureMap;
	public TerrainType[,] chosenHeightTerrainTypes;
	public TerrainType[,] chosenHeatTerrainTypes;
	public TerrainType[,] chosenMoistureTerrainTypes;
	public Biome[,] chosenBiomes;

	public TileData(float[,] heightMap, float[,] heatMap, float[,] moistureMap,
		TerrainType[,] chosenHeightTerrainTypes, TerrainType[,] chosenHeatTerrainTypes, TerrainType[,] chosenMoistureTerrainTypes,
		Biome[,] chosenBiomes)
	{
		this.heightMap = heightMap;
		this.heatMap = heatMap;
		this.moistureMap = moistureMap;
		this.chosenHeightTerrainTypes = chosenHeightTerrainTypes;
		this.chosenHeatTerrainTypes = chosenHeatTerrainTypes;
		this.chosenMoistureTerrainTypes = chosenMoistureTerrainTypes;
		this.chosenBiomes = chosenBiomes;
	}
}

public class LevelData
{
	public int gridSize;

	public TileData[,] tilesData;
	public int[,] treeData;

	public LevelData(int gridSize, int MapWidth, int MapHeight)
	{
		// build the tilesData matrix based on the level depth and width
		tilesData = new TileData[MapWidth * gridSize, MapHeight * gridSize];

		this.gridSize = gridSize;
	}

	public void AddTileData(TileData tileData, int tileXIndex, int tileYIndex)
	{
		// save the TileData in the corresponding coordinate
		tilesData[tileXIndex, tileYIndex] = tileData;
	}

	public WholeMapPos ConvertToWholeMapPos(int xIndex, int yIndex)
	{
		// the tile index is calculated by dividing the index by the number of tiles in that axis
		int tileXIndex = (int)Mathf.Floor((float)xIndex / (float)this.gridSize);
		int tileYIndex = (int)Mathf.Floor((float)yIndex / (float)this.gridSize);
		// the coordinate index is calculated by getting the remainder of the division above
		// we also need to translate the origin to the bottom left corner
		int wholeMapXIndex = xIndex % this.gridSize;
		int wholeMapYIndex = yIndex % this.gridSize;

		WholeMapPos MapPos = new WholeMapPos(tileXIndex, tileYIndex, wholeMapXIndex, wholeMapYIndex);
		return MapPos;
	}
}

public class WholeMapPos
{
	public int tileXIndex;
	public int tileYIndex;
	public int WholeMapXIndex;
	public int WholeMapYIndex;

	public WholeMapPos(int tileXIndex, int tileYIndex, int WholeMapXIndex, int WholeMapYIndex)
	{
		this.tileXIndex = tileXIndex;
		this.tileYIndex = tileYIndex;
		this.WholeMapXIndex = WholeMapXIndex;
		this.WholeMapYIndex = WholeMapYIndex;
	}
}
