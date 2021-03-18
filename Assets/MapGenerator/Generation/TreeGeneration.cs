using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
	[SerializeField]
	private Wave[] waves;

	[SerializeField]
	private float levelScale;

	[SerializeField]
	private int[] neighborRadius;

	[Range(0, 5)]
	[SerializeField]
	private int minTreeDistance;

	[Range(0, 5)]
	[SerializeField]
	private int randomizeTreeValue;

	[SerializeField]
	private GameObject treePrefab;

	public void GenerateTrees(int wholeMapWidth, int wholeMapHeight, float distanceBetweenGrid, LevelData levelData)
	{
		// generate a tree noise map using Perlin Noise
		float[,] treeMap = NoiseMapGeneration.GeneratePerlinNoiseMap(levelScale, wholeMapWidth, 0, 0, this.waves);
		int[,] treeData = new int[wholeMapHeight, wholeMapWidth];

		for(int y=0;y<wholeMapHeight;y++)
        {
			for (int x = 0; x < wholeMapWidth; x++)
			{
				treeData[x, y] = 0;
			}
		}

		//float levelSizeX = levelWidth * distanceBetweenGrid;
		//float levelSizeZ = levelDepth * distanceBetweenGrid;

		for (int yIndex = 0; yIndex < wholeMapHeight; yIndex++)
		{
			for (int xIndex = 0; xIndex < wholeMapWidth; xIndex++)
			{
				// convert from Level Coordinate System to Tile Coordinate System and retrieve the corresponding TileData
				WholeMapPos wholeMapPos = levelData.ConvertToWholeMapPos(xIndex, yIndex);
				TileData tileData = levelData.tilesData[wholeMapPos.tileXIndex, wholeMapPos.tileYIndex];
				int tileWidth = tileData.heightMap.GetLength(0);

				//// calculate the mesh vertex index
				//Vector3[] meshVertices = tileData.mesh.vertices;
				//int vertexIndex = wholeMapPos.WholeMapXIndex * tileWidth + wholeMapPos.WholeMapYIndex;

				// get the terrain type of this coordinate
				TerrainType terrainType = tileData.chosenHeightTerrainTypes[wholeMapPos.WholeMapXIndex, wholeMapPos.WholeMapYIndex];

				Biome biome = tileData.chosenBiomes[wholeMapPos.WholeMapXIndex, wholeMapPos.WholeMapYIndex];

				// check if it is a water terrain. Trees cannot be placed over the water
				if (!terrainType.name.Equals("water"))
				{
					float treeValue = treeMap[xIndex, yIndex];

					int terrainTypeIndex = terrainType.tileIndex;

					// compares the current tree noise value to the neighbor ones
					int neighborXBegin = (int)Mathf.Max(0, xIndex - this.neighborRadius[biome.tileIndex]);
					int neighborXEnd = (int)Mathf.Min(wholeMapWidth - 1, xIndex + this.neighborRadius[biome.tileIndex]);

					int neighborYBegin = (int)Mathf.Max(0, yIndex - this.neighborRadius[biome.tileIndex]);
					int neighborYEnd = (int)Mathf.Min(wholeMapHeight - 1, yIndex + this.neighborRadius[biome.tileIndex]);

					float maxValue = 0f;
					for (int neighborY = neighborYBegin; neighborY <= neighborYEnd; neighborY++)
					{
						for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++)
						{
							float neighborValue = treeMap[neighborX, neighborY];

							// saves the maximum tree noise value in the radius
							if (neighborValue >= maxValue)
							{
								maxValue = neighborValue;
							}
						}
					}
					//Debug.LogError($"{maxValue} = {treeValue}");
					// if the current tree noise value is the maximum one, place a tree in this location
					if (treeValue.Equals(maxValue))
					{
						bool findTree = false;

						int calculateDistance = Random.Range(minTreeDistance, minTreeDistance + randomizeTreeValue + 1);

						int treeXBegin = (int)Mathf.Max(0, xIndex - calculateDistance);
						int treeXEnd = (int)Mathf.Min(wholeMapWidth - 1, xIndex + calculateDistance);
						int treeYBegin = (int)Mathf.Max(0, yIndex - calculateDistance);
						int treeYEnd = (int)Mathf.Min(wholeMapHeight - 1, yIndex + calculateDistance);

						for (int treeY = treeYBegin; treeY <= treeYEnd; treeY++)
						{
							for (int treeX = treeXBegin; treeX <= treeXEnd; treeX++)
							{
								int treedata = treeData[treeX, treeY];

								if (treedata > 0)
								{
									findTree = true;
									break;
								}
							}
							if(findTree.Equals(true))
                            {
								treeData[xIndex, yIndex] = 0;
								break;
                            }
						}

						if(findTree.Equals(false))
                        {
							float halfGridTrash = levelData.gridSize % 2f;
							float halfGridSize = levelData.gridSize / 2f;
							float GridOffset = halfGridTrash == 0 ? (halfGridSize - 0.5f) : Mathf.Floor(halfGridSize);
							Vector3 treePosition = new Vector3(((float)xIndex - GridOffset) * distanceBetweenGrid, ((float)yIndex - GridOffset) * distanceBetweenGrid, -1f);

							GameObject tree = Instantiate(this.treePrefab) as GameObject;
							tree.transform.position = treePosition;
							tree.transform.localScale = new Vector3(distanceBetweenGrid / 2f, distanceBetweenGrid, distanceBetweenGrid);

							treeData[xIndex, yIndex] = 1;
						}
					}
					else
                    {
						treeData[xIndex, yIndex] = 0;
					}
				}
			}
		}
	}
}
