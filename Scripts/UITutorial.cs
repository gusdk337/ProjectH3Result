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
