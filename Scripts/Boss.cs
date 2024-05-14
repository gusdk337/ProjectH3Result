using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Animator anim;

    public int maxHp = 100;
    public float currentHp;

    public Slider healthSlider;

    public Material normalMat;  //평소 머테리얼
    public Material hitMat;     //피격시 머테리얼

    public Renderer[] bossRenderers;

    private bool isDead;

    //패턴
    public float attackCoolTime = 2f;
    public int damage = 10;
    private float lastAttackTime;
    private bool StartCoroutinePattern1 = false;
    private bool StartCoroutinePattern2 = false;
    private bool StartCoroutinePattern3 = false;
    private bool StartCoroutinePattern4 = false;
    private bool StartCoroutinePattern5 = false;

    //NavMesh
    private NavMeshAgent agent;

    //이펙트
    public ParticleSystem jump;

    //플레이어 위치
    private Transform playerTrans;

    private bool isFollowingPlayer = false;

    public GameObject item;

    private bool isEventTriggered = false;


    void Start()
    {
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.HitBoss, HitBoss);
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.DoubleHitBoss, DoubleHitBoss);

        this.jump.Stop();

        this.anim = GetComponent<Animator>();
        this.anim.SetInteger("State", 0);

        this.currentHp = this.maxHp;

        this.healthSlider = GameObject.Find("BossHealthSlider").GetComponent<Slider>();
        this.healthSlider.interactable = false;

        //this.bossRenderers = GetComponentsInChildren<Renderer>();

        this.lastAttackTime = Time.time;

        GameObject player = GameObject.Find("Player");
        this.playerTrans = player.transform;
        this.agent = GetComponent<NavMeshAgent>();
        StartCoroutine(this.MoveRoutine());
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            SetDestination(playerTrans.position);
        }

        this.ExecuteAttackPattern();

        if (IsAnimationPlaying())
        {
            this.StopMove();
        }

    }

    public IEnumerator MoveRoutine()
    {
        isFollowingPlayer = true;

        while (true)
        {
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        if (agent != null)
        {
            agent.SetDestination(destination);
        }
    }


    public void Attack(float damage)
    {
        this.currentHp -= damage;
        UpdateHealthUI();

        if(currentHp <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    IEnumerator Die()
    {
        if (this.gameObject.activeSelf && this.gameObject != null)
        {
            if (!isEventTriggered)
            {
                this.isDead = true;

                Debug.Log("보스 죽음");
                yield return StartCoroutine(FlashRed());

                this.anim.SetInteger("State", 3);

                yield return null;

                this.agent.isStopped = true;

                Instantiate(item, this.gameObject.transform.position, Quaternion.identity);

                EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.BossCut);

                isEventTriggered = true;

                yield return new WaitForSeconds(3f);



                this.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHealthUI()
    {
        this.healthSlider.value = this.currentHp;
    }

    IEnumerator FlashRed()
    {
        foreach (Renderer bossRenderer in bossRenderers)
        {
            bossRenderer.material = hitMat;
            this.anim.SetInteger("State", 2);
        }

        yield return new WaitForSeconds(0.1f);

        if (!isDead)
        {
            foreach (Renderer bossRenderer in bossRenderers)
            {
                bossRenderer.material = normalMat;
                this.anim.SetInteger("State", 0);
            }
        }
    }

    public void ExecuteAttackPattern()
    {
        if (Time.time - lastAttackTime > attackCoolTime)
        {
            if (!IsPatternRunning())
            {
                int randomPatternIndex = Random.Range(0, 5);

                switch (randomPatternIndex)
                {
                    case 0:
                        StartCoroutine(Pattern1());
                        break;

                    case 1:
                        StartCoroutine(Pattern2());
                        break;

                    case 2:
                        StartCoroutine(Pattern3());
                        break;

                    case 3:
                        StartCoroutine(Pattern4());
                        break;

                    case 4:
                        StartCoroutine(Pattern5());
                        break;


                }
            }
        }
    }

    IEnumerator Pattern1()
    {
        StartCoroutinePattern1 = true;

        this.Move();

        yield return new WaitForSeconds(2f); // 패턴 지속 시간

        this.StopMove();

        StartCoroutinePattern1 = false;
    }

    IEnumerator Pattern2()
    {
        StartCoroutinePattern2 = true;


        this.anim.SetInteger("State", 4);
        yield return new WaitForSeconds(1f);
        this.jump.Play();

        yield return new WaitForSeconds(1.5f); // 패턴 지속 시간

        this.jump.Stop();
        this.anim.SetInteger("State", 0);

        StartCoroutinePattern2 = false;
    }
    IEnumerator Pattern3()
    {
        StartCoroutinePattern3 = true;


        this.anim.SetInteger("State", 5);

        yield return new WaitForSeconds(1.5f); // 패턴 지속 시간

        this.anim.SetInteger("State", 0);

        StartCoroutinePattern3 = false;
    }
    IEnumerator Pattern4()
    {
        StartCoroutinePattern4 = true;


        this.anim.SetInteger("State", 6);

        yield return new WaitForSeconds(1.5f); // 패턴 지속 시간

        this.anim.SetInteger("State", 0);

        StartCoroutinePattern4 = false;
    }
    IEnumerator Pattern5()
    {
        StartCoroutinePattern5 = true;


        this.anim.SetInteger("State", 7);

        yield return new WaitForSeconds(1.5f); // 패턴 지속 시간

        this.anim.SetInteger("State", 0);

        StartCoroutinePattern5 = false;
    }

    public void Move()
    {
        this.agent.isStopped = false;

        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        this.agent.SetDestination(randomPosition);

        this.anim.SetInteger("State", 1);

    }

    public void StopMove()
    {
        this.agent.isStopped = true;

        this.anim.SetInteger("State", 0);
    }

    public bool IsPatternRunning()
    {
        return StartCoroutinePattern1 || StartCoroutinePattern2;
    }

    public bool IsAnimationPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("b1HitFront2");
    }

    public void HitBoss(short type)
    {
        if(this.gameObject != null)
        {
            this.Attack(2.5f);
        }

    }
    public void DoubleHitBoss(short type)
    {
        this.Attack(5f);
    }

    private void OnDisable()
    {
        Destroy(this);
    }
}
