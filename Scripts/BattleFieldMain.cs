using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleFieldMain : MonoBehaviour
{
    public UIBattleFieldDirector director;

    public int inactiveCount;

    public GameObject bossPrefab;

    public GameObject player;

    //public int totalWaves = 3;
    private int currentWave = 0;

    private float fadeSpeed = 0.5f;

    public GameObject lightFx;
    public GameObject park;

    public AudioSource audio;

    public void Init()
    {

    }

    private void Start()
    {
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.HitMob, HitMob);
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.EnterPark, EnterPark);
        EventDispatcher.instance.AddEventHandler((int)EventEnum.eEventType.BossCut, BossCut);

        this.WaveOne();

    }

    private void Update()
    {
        Cursor.visible = false; //마우스 커서 숨기기

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.collider.CompareTag("Mob"))
        //        {
        //            this.inactiveCount++;
        //            this.director.healthSlider.value -= 1;
        //            Debug.Log(inactiveCount);

        //            hit.collider.gameObject.SetActive(false);
        //        }
        //    }
        //}

        if (this.currentWave == 1)
        {
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

                //StartCoroutine(FadeOut());


                this.director.healthSlider.gameObject.SetActive(false);

                this.lightFx.gameObject.SetActive(true);
                this.park.SetActive(true);

                //this.director.bossHealthSlider.gameObject.SetActive(true);
                //this.WaveTwo();
            }
        }
        //else if(this.currentWave == 2)   
        //{
        //    if (this.inactiveCount == 3)   //1-2웨이브 끝나면
        //    {
        //        this.inactiveCount = 0;
        //        Destroy(this.mobSpawnPoint2);
        //        //this.mobSpawnPoint2.gameObject.SetActive(false);

        //        this.WaveThree();
        //    }
        //}
        //else if (this.currentWave == 3)
        //{
        //    if (this.inactiveCount == 7)   //1-3웨이브 끝나면
        //    {
        //        this.inactiveCount = 0;
        //        this.mobSpawnPoint3.gameObject.SetActive(false);
        //    }
        //}


    }

    public void WaveOne()   //1웨이브
    {
        //StartCoroutine(this.SpawnMobs(5, 0.3f));
        //StartCoroutine(mobSpawnPoint.SpawnMobs(5, 0.3f));
        this.currentWave++;
    }

    public void WaveTwo()   //1-2웨이브
    {
        //StartCoroutine(this.SpawnMobs(10, 0.3f));
        this.currentWave++;
    }

    public void WaveThree()   //1-3웨이브
    {
        //StartCoroutine(this.SpawnMobs(20, 0.3f));
        this.currentWave++;
    }

    //IEnumerator SpawnMobs(int count, float delay)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (mobSpawnPoint[0].isEnter)
    //        {
    //            Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
    //            Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
    //            yield return new WaitForSeconds(delay);
    //        }
    //    }
    //}

    IEnumerator FadeIn()
    {
        float alpha = 1f;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            this.director.imgFade.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }


    }

    IEnumerator FadeOut()
    {
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            this.director.imgFade.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        StartCoroutine(FadeIn());
        this.SpawnBoss();
        this.player.transform.position = new Vector3(0, 0, 130f);

    }
    public void SpawnBoss()
    {
            Instantiate(bossPrefab, new Vector3(0f, 0f, 180f), Quaternion.identity);

            this.audio.Play();
    }

    public void HitMob(short type)
    {
        this.inactiveCount++;
        this.director.healthSlider.value -= 1;
        Debug.Log(inactiveCount);
    }

    public void EnterPark(short type)
    {
        if(this.lightFx.gameObject != null)
        {
            this.lightFx.gameObject.SetActive(false);

            this.SpawnBoss();

            this.director.bossHealthSlider.gameObject.SetActive(true);
        }

    }

    public void BossCut(short type)
    {
        StartCoroutine(this.ReturnToOutPost());
    }

    IEnumerator ReturnToOutPost()
    {
        yield return new WaitForSeconds(15f);

        EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeToOutPost);

    }

    private void OnDisable()
    {
        Destroy(this);
    }

}
