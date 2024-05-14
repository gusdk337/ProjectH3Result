using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOutPostDirectorTuto : MonoBehaviour
{
    //public Button btnGoBattleField;

    public GameObject UIInventory;
    public GameObject UIBuildNotice;

    public UITutorial UITutorial;
    void Start()
    {
        //this.btnGoBattleField.onClick.AddListener(() =>
        //{
        //    EventDispatcher.instance.SendEvent((int)EventEnum.eEventType.ChangeToBattleField);
        //});

        this.UIInventory.GetComponent<UIInventory>().Init();
    }
    private void Update()
    {
        StartCoroutine(ShowUIInventory());
    }

    IEnumerator ShowUIInventory()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            this.UIInventory.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            this.UIInventory.gameObject.SetActive(false);
        }

        yield return null;

    }
    public void ShowUIBuildNotice()
    {
        this.UIBuildNotice.gameObject.SetActive(true);
    }

    public void HideUIBuildNotice()
    {
        this.UIBuildNotice.gameObject.SetActive(false);
    }

}
