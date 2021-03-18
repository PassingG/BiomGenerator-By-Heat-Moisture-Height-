using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainType
{
    public int tileIndex;
	public string name;
	public float value;
	public Color color;
}

[System.Serializable]
public class Biome
{
    public int tileIndex;
    public string name;
    public Color color;
}

[System.Serializable]
public class BiomeRow
{
    public Biome[] biomes;
}

public class TileGeneration : MonoBehaviour
{
    enum VisualizationMode { Height, Heat, Moisture, Biome }

    [SerializeField]
    private BiomeRow[] biomes;
    
    [SerializeField]
	private Wave[] heightWaves;
	[SerializeField]
	private Wave[] heatWaves;
    [SerializeField]
    private Wave[] moistureWaves;

    [SerializeField]
	private TerrainType[] heightTerrainTypes;
	[SerializeField]
	private TerrainType[] heatTerrainTypes;
    [SerializeField]
    private TerrainType[] moistureTerrainTypes;

    [SerializeField]
	private MeshRenderer tileRenderer;

	[SerializeField]
	private MeshFilter meshFilter;

	[SerializeField]
	private float mapScale;

	[SerializeField]
	private VisualizationMode visualizationMode;

	[SerializeField]
	private AnimationCurve heatCurve;
    [SerializeField]
    private AnimationCurve moistureCurve;
    public TileData GenerateTile(int gridSize, int mapX, int mapY, float hitMapPos, int mapCountY)
	{
		// calculate the offsets based on the tile position
		float offsetX = mapX * (gridSize - 1);
        float offsetY = mapY * (gridSize - 1);

        // generate a heightMap using Perlin Noise
        float[,] heightMap = NoiseMapGeneration.GeneratePerlinNoiseMap(this.mapScale, gridSize, offsetX, offsetY, this.heightWaves);

        float distanceBetweenGrid = 1f / ((float)gridSize - 1f);
        float vertexOffsetY = this.gameObject.transform.position.y / distanceBetweenGrid;

        float doublefloat = 1.5f;
        float maxDistanceY = (mapCountY * gridSize) / (2f * doublefloat);
        float centerVertexY = (maxDistanceY * hitMapPos) * doublefloat;
        // generate a heatMap using uniform noise
        float[,] uniformHeatMap = NoiseMapGeneration.GenerateUniformNoiseMap(gridSize, centerVertexY, maxDistanceY, vertexOffsetY);
        float[,] randomHeatMap = NoiseMapGeneration.GeneratePerlinNoiseMap(this.mapScale * 10f, gridSize, offsetX, offsetY, this.heatWaves);
        float[,] heatMap = new float[gridSize, gridSize];

        for (int yIndex = 0; yIndex < gridSize; yIndex++)
        {
            for (int xIndex = 0; xIndex < gridSize; xIndex++)
            {
                heatMap[xIndex, yIndex] = uniformHeatMap[xIndex, yIndex] * randomHeatMap[xIndex, yIndex];
                heatMap[xIndex, yIndex] += heightMap[xIndex, yIndex] * heightMap[xIndex, yIndex];
            }
        }

        // generate a moistureMap using Perlin Noise
        float[,] moistureMap = NoiseMapGeneration.GeneratePerlinNoiseMap(mapScale, gridSize, offsetX, offsetY, this.moistureWaves);
        for (int yIndex = 0; yIndex < gridSize; yIndex++)
        {
            for (int xIndex = 0; xIndex < gridSize; xIndex++)
            {
                moistureMap[xIndex, yIndex] -= this.moistureCurve.Evaluate(heightMap[xIndex, yIndex]) * heightMap[xIndex, yIndex];
            }
        }

        TerrainType[,] chosenHeightTerrainTypes = new TerrainType[gridSize, gridSize];
        Texture2D heightTexture = BuildTexture(heightMap, this.heightTerrainTypes, chosenHeightTerrainTypes);

        TerrainType[,] chosenHeatTerrainTypes = new TerrainType[gridSize, gridSize];
        Texture2D heatTexture = BuildTexture(heatMap, this.heatTerrainTypes, chosenHeatTerrainTypes);

        TerrainType[,] chosenMoistureTerrainTypes = new TerrainType[gridSize, gridSize];
        Texture2D moistureTexture = BuildTexture(moistureMap, this.moistureTerrainTypes, chosenMoistureTerrainTypes);

        Biome[,] chosenBiomes = new Biome[gridSize, gridSize];
        Texture2D biomTexture = BuildBiomeTexture(chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes, chosenBiomes);

        switch (this.visualizationMode)
        {
            case VisualizationMode.Height:
                this.tileRenderer.material.mainTexture = heightTexture;
                break;
            case VisualizationMode.Heat:
                this.tileRenderer.material.mainTexture = heatTexture;
                break;
            case VisualizationMode.Moisture:
                this.tileRenderer.material.mainTexture = moistureTexture;
                break;
            case VisualizationMode.Biome:
                // assign material texture to be the moistureTexture
                this.tileRenderer.material.mainTexture = biomTexture;
                break;
        }

        TileData tileData = new TileData(heightMap, heatMap, moistureMap, chosenHeightTerrainTypes, chosenHeatTerrainTypes, chosenMoistureTerrainTypes, chosenBiomes);

        return tileData;
    }

    TerrainType ChooseTerrainType(float height, TerrainType[] terrainType)
	{
		foreach (TerrainType Type in terrainType)
		{
			if (height < Type.value)
			{
				return Type;
			}
		}
		return terrainType[terrainType.Length - 1];
	}

    private Texture2D BuildTexture(float[,] heightMap, TerrainType[] terrainTypes, TerrainType[,] chosenTerrainTypes)
    {
        int tileX = heightMap.GetLength(0);
        int tileY = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileY * tileX];
        for (int yIndex = 0; yIndex < tileY; yIndex++)
        {
            for (int xIndex = 0; xIndex < tileX; xIndex++)
            {
                int colorIndex = yIndex * tileX + xIndex;
                float height = heightMap[xIndex, yIndex];

                TerrainType terrainType = ChooseTerrainType(height, terrainTypes);

                colorMap[colorIndex] = terrainType.color;

                chosenTerrainTypes[xIndex, yIndex] = terrainType;
            }
        }

        Texture2D tileTexture = new Texture2D(tileX, tileY);
        tileTexture.filterMode = FilterMode.Point;
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();

        return tileTexture;
    }

    private Texture2D BuildBiomeTexture(TerrainType[,] heightTerrainTypes, TerrainType[,] heatTerrainTypes, TerrainType[,] moistureTerrainTypes, Biome[,] chosenBiomes)
    {
        int tileX = heatTerrainTypes.GetLength(0);
        int tileY = heatTerrainTypes.GetLength(1);

        Color[] colorMap = new Color[tileY * tileX];
        for (int yIndex = 0; yIndex < tileY; yIndex++)
        {
            for (int xIndex = 0; xIndex < tileX; xIndex++)
            {
                int colorIndex = yIndex * tileX + xIndex;

                TerrainType heightTerrainType = heightTerrainTypes[xIndex, yIndex];
                // check if the current coordinate is a water region
                if (heightTerrainType.name != "water")
                {
                    // if a coordinate is not water, its biome will be defined by the heat and moisture values
                    TerrainType heatTerrainType = heatTerrainTypes[xIndex, yIndex];
                    TerrainType moistureTerrainType = moistureTerrainTypes[xIndex, yIndex];

                    // terrain type index is used to access the biomes table
                    Biome biome = this.biomes[moistureTerrainType.tileIndex].biomes[heatTerrainType.tileIndex];
                    // assign the color according to the selected biome
                    colorMap[colorIndex] = biome.color;

                    chosenBiomes[xIndex, yIndex] = biome;
                }
                else
                {
                    // water regions don't have biomes, they always have the same color
                    colorMap[colorIndex] = heightTerrainType.color;
                }
            }
        }

        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D(tileX, tileY);
        tileTexture.filterMode = FilterMode.Point;
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();

        return tileTexture;
    }
}
