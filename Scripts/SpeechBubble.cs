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
