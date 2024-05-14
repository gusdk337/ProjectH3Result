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
            yield return new WaitForSeconds(0.05f); // �� ���ھ� ǥ�õǴ� ���� ����
        }
    }
}
