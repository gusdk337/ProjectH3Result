using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnPoint2 : MonoBehaviour
{
    public GameObject[] mobPrefab;
    public Mob[] mob;

    private int cntEnter = 0;
    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("µé¾î¿È");

            this.cntEnter++;

            if (this.cntEnter == 1)
            {
                StartCoroutine(this.SpawnMobs(10, 0.3f));
            }

            for (int i = 0; i < 10; i++)
            {
                StopCoroutine(mob[i].MoveToBack());
                StartCoroutine(mob[i].MoveRoutine());
                EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeAnimation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("³ª°¨");

            for (int i = 0; i < 10; i++)
            {
                StopCoroutine(mob[i].MoveRoutine());
                StartCoroutine(mob[i].MoveToBack());
                EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeAnimation);
            }
        }
    }

    public IEnumerator SpawnMobs(int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            //Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
            //Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
            this.mobPrefab[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
        }
    }

}
