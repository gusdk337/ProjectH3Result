using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    public Animator anim;

    private Transform playerTrans;
    private NavMeshAgent agent;

    private bool isFollowingPlayer = false;

    void Start()
    {
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.ChangeAnimation, ChangeAnimation);

        this.anim = GetComponent<Animator>();
        this.anim.SetInteger("State", 1);

        GameObject player = GameObject.Find("Player");
        this.playerTrans = player.transform;
        this.agent = GetComponent<NavMeshAgent>();
        StartCoroutine(this.MoveRoutine());
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            this.agent.enabled = true;

            SetDestination(playerTrans.position);
        }
    }

    public IEnumerator MoveRoutine()    //플레이어를 따라가는 코루틴
    {
        isFollowingPlayer = true;

        while (true) 
        {
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(destination);
        }
    }

    public IEnumerator MoveToBack() //플레이어를 따라가는 것을 멈추고 돌아가는 코루틴
    {
        this.agent.enabled = false;
        isFollowingPlayer = false;
        Transform parentTransform = transform.parent;


        while (Vector3.Distance(parentTransform.InverseTransformPoint(transform.position), Vector3.zero) > 0.1f)
        {
            float randomX = Random.Range(-27f, 40f);
            float randomZ = Random.Range(-27f, 20f);

            this.agent.enabled = true;

            SetDestination(parentTransform.TransformPoint(randomX, 0, randomZ));

            yield return new WaitForSeconds(1f);
        }
    }

    public void ChangeAnimation(short type)
    {
        if(this.anim != null)
        {
            if (this.anim.GetInteger("State") == 0)
            {
                Debug.Log("1로 바뀜");
                this.anim.SetInteger("State", 1);
            }
            else if (this.anim.GetInteger("State") == 1)
            {
                Debug.Log("0으로 바뀜");
                this.anim.SetInteger("State", 0);
            }
        }
    }
}
