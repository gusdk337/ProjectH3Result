using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleFieldMainTuto : MonoBehaviour
{
    public UIBattleFieldDirector director;

    public int inactiveCount;

    public GameObject bossPrefab;

    public GameObject player;

    private int currentWave = 0;

    private float fadeSpeed = 0.5f;

    public GameObject lightFx;
    public GameObject park;

    public AudioSource audio;


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


                this.director.healthSlider.gameObject.SetActive(false);

                StartCoroutine(this.WaveCutDialogue());
                this.lightFx.gameObject.SetActive(true);
                this.park.SetActive(true);
            }
        }
    }

    public void WaveOne()   //1웨이브
    {
        this.currentWave++;
    }

    public void WaveTwo()   //1-2웨이브
    {
        this.currentWave++;
    }

    public void WaveThree()   //1-3웨이브
    {
        this.currentWave++;
    }

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
        this.director.healthSlider.value -= 1;  //웨이브 바 감소
        Debug.Log(inactiveCount);
    }

    public void EnterPark(short type)
    {
        if (this.lightFx.gameObject != null)
        {

            this.lightFx.gameObject.SetActive(false);

            this.SpawnBoss();

            //SoundManager.PlaySFX("08_Battlestations", true);

            this.director.bossHealthSlider.gameObject.SetActive(true);
        }

    }

    IEnumerator WaveCutDialogue()
    {
        yield return new WaitForSeconds(2f);

        this.director.uiTutorial.speechBubble1.SetActive(true);
    }

    public void BossCut(short type)
    {
        StartCoroutine(this.ReturnToOutPost());
        StartCoroutine(this.BossClearDialogue());
    }

    IEnumerator ReturnToOutPost()
    {
        yield return new WaitForSeconds(15f);

        EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeToOutPost);

    }


    IEnumerator BossClearDialogue()
    {
        yield return new WaitForSeconds(3f);

        this.director.uiTutorial.speechBubble2.SetActive(true);
    }


}
