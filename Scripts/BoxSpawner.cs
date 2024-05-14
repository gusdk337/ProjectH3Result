using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;    //상자 프리팹
    public Transform[] spawnPositions;  //상자 스폰 포인트들
    public int boxCnt = 3;

    void Start()
    {
        SpawnRandomBoxes();
    }

    void SpawnRandomBoxes()
    {
        var usedTransforms = new List<Transform>();

        for (int i = 0; i < boxCnt; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, spawnPositions.Length);
            } while (usedTransforms.Contains(spawnPositions[randomIndex]));

            usedTransforms.Add(spawnPositions[randomIndex]);

            Instantiate(boxPrefab, spawnPositions[randomIndex].position, Quaternion.identity);
        }
    }
}
