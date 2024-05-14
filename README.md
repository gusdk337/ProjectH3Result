# <p align="center">Project H3</p>

<p align="center">
<img src="https://github.com/gusdk337/gusdk337/assets/51481890/4b2f76c4-0466-46db-bea5-f289b4430702" width="200">
</p>

## 🎮게임 소개
전투장에서 재료를 구해 전초기지를 짓는 게임 &nbsp;

- 장르: 액션
- 특징: 랜타디 IP 활용
- 비고: 2023 대구X엔젤 인디게임 페스티벌 공모전 지정 주제 예선 통과

&nbsp;

## 👩🏻‍💻개발 기간 & 개발 인원
- 개발 기간: 2023.11.16~2023.12.18(약 4주)
- 개발 인원: 3인(아트 1, 프로그래머 2)

&nbsp;

## ▶️플레이 영상
https://youtu.be/NBXE17kX0nQ

&nbsp;

## ✏️팀 내 맡은 역할
- 팀장
- 일반 몬스터 AI
- 보스 몬스터 AI
- 웨이브
- 구조물 랜덤 배치
- 도우미 로봇

&nbsp;

## ❗일반 몬스터 AI
- 일반 몬스터 스폰 구역에 가까이 가면 몬스터 생성 후 플레이어에게로 이동
- 스폰 구역에서 멀어지면 몬스터가 제자리로 돌아감
  
![일반몬스터](https://github.com/gusdk337/ProjectH3Result/assets/51481890/6e076266-9a9f-457f-91c5-3be2c650fad1)

![일반몬스터1](https://github.com/gusdk337/ProjectH3Result/assets/51481890/847aacbc-63f7-4781-96de-c285da63f4ba)

<details>
 <summary>코드 보기</summary>
 
```ts
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

```
▲ Mob 스크립트

```ts
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

```
▲ MobSpawnPoint1 스크립트
</details>

## ❗보스 몬스터 AI
- 웨이브 클리어 시 보스 몬스터 소환 가능
  
![보스몬스터](https://github.com/gusdk337/ProjectH3Result/assets/51481890/252e25bc-6929-451e-9bb9-f53ffcc14017)

<details>
 <summary>코드 보기</summary>
 
```ts
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

```
▲ Boss 스크립트
</details>

## ❗웨이브
- 전투장 입장 시 웨이브 바 활성화
- 일반 몬스터 처치 시 웨이브 바 감소
- 웨이브 클리어 시 보스 몬스터 소환 가능
  
![웨이브](https://github.com/gusdk337/ProjectH3Result/assets/51481890/44187f57-8d4f-4830-8509-3362f9b1ecc6)

<details>
 <summary>코드 보기</summary>
 
```ts
    private void Start()
    {
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.HitMob, HitMob);
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.EnterPark, EnterPark);
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.BossCut, BossCut);


        this.WaveOne();

    }

    if (this.inactiveCount >= 30)   //웨이브 끝나면
    {
        this.inactiveCount = 0;

        GameObject.Find("1(Clone)").gameObject.SetActive(false);
        GameObject.Find("2(Clone)").gameObject.SetActive(false);
        GameObject.Find("3(Clone)").gameObject.SetActive(false);
        GameObject.Find("4(Clone)").gameObject.SetActive(false);
        GameObject.Find("5(Clone)").gameObject.SetActive(false);
        GameObject.Find("6(Clone)").gameObject.SetActive(false);
        GameObject.Find("7(Clone)").gameObject.SetActive(false);


        this.director.healthSlider.gameObject.SetActive(false);

        StartCoroutine(this.WaveCutDialogue());
        this.lightFx.gameObject.SetActive(true);
        this.park.SetActive(true);
    }

    public void HitMob(short type)
    {
        this.inactiveCount++;
        this.director.healthSlider.value -= 1;  //웨이브 바 감소
        Debug.Log(inactiveCount);
    }


```
▲ BattleFieldMainTuto 스크립트 중 일부

```ts
    public void UpdateHealthUI()
    {
        this.healthSlider.value = this.currentHp;
    }
```
▲ Boss 스크립트 중 일부
</details>

## ❗구조물 랜덤 배치 
- 전투장 입장 시 아이템 상자 랜덤 배치
  
![구조물](https://github.com/gusdk337/ProjectH3Result/assets/51481890/d08ee73b-857a-4e16-a64a-1deba7b58bc0)

<details>
 <summary>코드 보기</summary>
 
```ts
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

```
▲ BoxSpawner 스크립트
</details>

## ❗도우미 로봇
- 우측 대사를 통해 플레이어의 행동에 도움을 줌
  
![도우미](https://github.com/gusdk337/ProjectH3Result/assets/51481890/6589c9ab-dbeb-48ed-af33-8871a790b7f5)

<details>
 <summary>코드 보기</summary>
 
```ts
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    public GameObject speechBubble0;
    public GameObject speechBubble1;
    public GameObject speechBubble2;

    private void Start()
    {
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.WaveCut, WaveCut);

        StartCoroutine(this.Dialogue());
    }

    IEnumerator Dialogue()
    {
        yield return new WaitForSeconds(3f);

        this.speechBubble0.SetActive(true);
    }
    public void WaveCut(short type)
    {
        StartCoroutine(this.BossDialogue());
    }

    IEnumerator BossDialogue()
    {
        yield return new WaitForSeconds(2f);

        this.speechBubble1.SetActive(true);
    }
}

```
▲ UITutorial 스크립트

```ts
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Text[] txt;

    private void Start()
    {
        StartCoroutine(this.txtPlay());
    }

    IEnumerator txtPlay()
    {
        for(int i = 0; i < txt.Length; i++)
        {
            txt[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            txt[i].gameObject.SetActive(false);

            if(i == txt.Length - 1)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}

```
▲ SpeechBubble 스크립트

```ts
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    public Text dialogueText;
    public string fullText;
    private int currentCharacterIndex;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        while (currentCharacterIndex < fullText.Length)
        {
            dialogueText.text += fullText[currentCharacterIndex];
            currentCharacterIndex++;
            yield return new WaitForSeconds(0.05f); // 한 글자씩 표시되는 간격 조절
        }
    }
}

```
▲ TextAnimation 스크립트

</details>
