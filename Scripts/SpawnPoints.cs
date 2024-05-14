using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public Transform[] spawnPos;
    public GameObject[] spawnPointPrefabs;

    private void Start()
    {
        this.RandomSpawn();
    }

    public void RandomSpawn()
    {
        List<int> availableIndices = new List<int>(spawnPos.Length);
        for (int i = 0; i < spawnPos.Length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < spawnPointPrefabs.Length; i++)
        {
            int randomPosIndex = Random.Range(0, availableIndices.Count);
            int spawnIndex = availableIndices[randomPosIndex];
            availableIndices.RemoveAt(randomPosIndex);

            GameObject spawnedObject = Instantiate(spawnPointPrefabs[i], spawnPos[spawnIndex].position, Quaternion.identity);

            spawnedObject.SetActive(true);

        }
    }
}
