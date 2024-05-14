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
        if (other.gameObject.CompareTag("Player"))  //�÷��̾ ���� ������ ������
        {
            Debug.Log("����");

            this.cntEnter++;

            if(this.cntEnter == 1)  //�� ó�� ������ �� ���� ����
            {
                StartCoroutine(this.SpawnMobs(10, 0.3f));
            }


            for (int i = 0; i < 10; i++)
            {
                StopCoroutine(mob[i].MoveToBack());
                StartCoroutine(mob[i].MoveRoutine());   //�÷��̾ ����
                EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeAnimation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  //�÷��̾ ���� ������ ������
        {
            Debug.Log("����");

            for (int i = 0; i < 10; i++)
            {
                StopCoroutine(mob[i].MoveRoutine());
                StartCoroutine(mob[i].MoveToBack());    //�÷��̾ ���󰡴� ���� ���߰� �ٽ� ���ư�
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
