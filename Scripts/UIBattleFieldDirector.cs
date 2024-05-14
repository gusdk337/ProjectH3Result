using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleFieldDirector : MonoBehaviour
{
    public Slider bossHealthSlider;
    public Slider healthSlider;

    public Image imgFade;

    public GameObject UIInventory;
    public GameObject roulette;

    public UITutorial uiTutorial;

    void Start()
    {
        this.healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        this.healthSlider.interactable = false;

        StartCoroutine(this.RouletteDisable());

        this.UIInventory.GetComponent<UIInventory>().Init();
    }
    private void Update()
    {
        StartCoroutine(this.ShowUIInventory());
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

    IEnumerator RouletteDisable()
    {
        yield return new WaitForSeconds(5f);

        this.roulette.SetActive(false);
    }
}
