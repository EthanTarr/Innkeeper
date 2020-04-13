using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfDayBehavior : MonoBehaviour
{
    public Transform LevelChoicesScreen;

    private Transform Player;
    
    // Start is called before the first frame update
    void Start()
    {
        if(LevelChoicesScreen == null)
        {
            Debug.LogError(name + " could not find Level Up Choice Selection Screen on Startup");
        }
        Player = GameObject.Find("Player").transform;
        if(Player == null)
        {
            Debug.LogError(name + " could not find Player on startup");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpEndOfDay(int SatisfiedCustomers, int DisSatisfiedCustomers, float PreviousXp, float CurrentXp)
    {

        StartCoroutine(SetUp(SatisfiedCustomers, DisSatisfiedCustomers, PreviousXp, CurrentXp));

    }

    IEnumerator SetUp(int SatisfiedCustomers, int DisSatisfiedCustomers, float PreviousXp, float CurrentXp)
    {
        this.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = SatisfiedCustomers + "";

        this.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text = DisSatisfiedCustomers + "";

        int SkillUpCount = 0;
        while (Mathf.Floor(CurrentXp / 750) > Mathf.Floor(PreviousXp / 750))
        {
            this.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().setSizeByPercentage(1, 5);
            yield return new WaitForSeconds(2.5f);
            this.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().resetXpBar();
            PreviousXp += 750 - (PreviousXp % 750);
            this.transform.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text =
            "Level " + (int.Parse(this.transform.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text.Substring(6)) + 1) + "";
            if ((int.Parse(this.transform.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text.Substring(6))) % 2 == 1)
            {
                SkillUpCount++;
            }
        }
        Debug.Log(CurrentXp != PreviousXp);
        if (CurrentXp != PreviousXp)
        {
            this.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().setSizeByPercentage((CurrentXp - PreviousXp) / 750, 5 * (CurrentXp - PreviousXp) / 750);
            yield return new WaitForSeconds(2.5f * (CurrentXp - PreviousXp) / 750);
            Debug.Log(CurrentXp != PreviousXp);
        }

        if (SkillUpCount > 0)
        {
            LevelChoicesScreen.GetComponent<LevelManager>().numOfSkilUps = SkillUpCount;
            LevelChoicesScreen.gameObject.SetActive(true);
        }
    }

    public void closeEndofDay()
    {
        if(!LevelChoicesScreen.gameObject.activeSelf)
        {
            Player.GetComponent<GameManager>().start();
            Player.GetComponent<GameManager>().BlackBackground.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
