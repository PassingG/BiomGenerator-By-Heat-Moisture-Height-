using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
	public float seed;
	public float frequency;
	public float amplitude;
}

public class NoiseMapGeneration : MonoBehaviour
{
	// 깊이와 넓이에 따른 맵 생성
	public static float[,] GeneratePerlinNoiseMap(float scale, int gridSize, float offsetX, float offsetY, Wave[] waves)
	{
		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[gridSize, gridSize];

		for (int yIndex = 0; yIndex < gridSize; yIndex++)
		{
			for (int xIndex = 0; xIndex < gridSize; xIndex++)
			{
				// calculate sample indices based on the coordinates, the scale and the offset
				float sampleX = (xIndex + offsetX) / scale;
				float sampleY = (yIndex + offsetY) / scale;

				float noise = 0f;
				float normalization = 0f;
				foreach (Wave wave in waves)
				{
					// generate noise value using PerlinNoise for a given Wave
					noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleY * wave.frequency + wave.seed);
					normalization += wave.amplitude;
				}
				// normalize the noise value so that it is within 0 and 1
				noise /= normalization;

				noiseMap[xIndex, yIndex] = noise;
			}
		}

		return noiseMap;
	}

	public static float[,] GenerateUniformNoiseMap(int gridSize, float centerVertexY, float maxDistanceY, float offsetY)
	{
		float[,] noiseMap = new float[gridSize, gridSize];

		for (int yIndex = 0; yIndex < gridSize; yIndex++)
		{
			float sampleY = yIndex + offsetY;

			float noise = Mathf.Abs(sampleY - centerVertexY) / maxDistanceY;

			for (int xIndex = 0; xIndex < gridSize; xIndex++)
			{
				noiseMap[xIndex, yIndex] = noise;
			}
		}

		return noiseMap;
	}
}
