using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnPoint1 : MonoBehaviour
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
        if (other.gameObject.CompareTag("Player"))  //플레이어가 구역 안으로 들어오면
        {
            Debug.Log("들어옴");

            this.cntEnter++;

            if(this.cntEnter == 1)  //맨 처음 들어왔을 때 몬스터 생성
            {
                StartCoroutine(this.SpawnMobs(10, 0.3f));
            }


            for (int i = 0; i < 10; i++)
            {
                StopCoroutine(mob[i].MoveToBack());
                StartCoroutine(mob[i].MoveRoutine());   //플레이어를 따라감
                EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeAnimation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  //플레이어가 구역 밖으로 나가면
        {
            Debug.Log("나감");

            for (int i = 0; i < 10; i++)
            {
                StopCoroutine(mob[i].MoveRoutine());
                StartCoroutine(mob[i].MoveToBack());    //플레이어를 따라가는 것을 멈추고 다시 돌아감
                EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeAnimation);
            }
        }
    }

    public IEnumerator SpawnMobs(int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            this.mobPrefab[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
        }
    }

}
