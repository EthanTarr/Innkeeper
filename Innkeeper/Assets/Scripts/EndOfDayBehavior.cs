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
        if (SatisfiedCustomers > 0)
        {
            this.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = SatisfiedCustomers + "";
            yield return new WaitForSeconds(1f);
        
            this.transform.GetChild(4).GetChild(2).gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

        if (DisSatisfiedCustomers > 0)
        {
            this.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text = DisSatisfiedCustomers + "";
            yield return new WaitForSeconds(1f);
        }

        int SkillUpCount = 0;
        while (Player.GetComponent<PlayerBehavior>().xpToLevels(CurrentXp) > Player.GetComponent<PlayerBehavior>().xpToLevels(PreviousXp))
        {
            this.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().setSizeByPercentage(1, 7.5f);
            yield return new WaitForSeconds(5f);
            this.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().resetXpBar();
            PreviousXp = Player.GetComponent<PlayerBehavior>().LevelMilestones[Player.GetComponent<PlayerBehavior>().xpToLevels(PreviousXp)];
            this.transform.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text =
            "Level " + (int.Parse(this.transform.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text.Substring(6)) + 1) + "";
            if ((int.Parse(this.transform.GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text.Substring(6))) % 2 == 1)
            {
                SkillUpCount++;
            }
        }
        if (CurrentXp != PreviousXp)
        {
            this.transform.GetChild(7).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().setSizeByPercentage(
                (CurrentXp - PreviousXp) / Player.GetComponent<PlayerBehavior>().LevelMilestones[Player.GetComponent<PlayerBehavior>().xpToLevels(CurrentXp)], 
                7.5f * (CurrentXp - PreviousXp) / Player.GetComponent<PlayerBehavior>().LevelMilestones[Player.GetComponent<PlayerBehavior>().xpToLevels(CurrentXp)]);
            yield return new WaitForSeconds(5f * (CurrentXp - PreviousXp) / Player.GetComponent<PlayerBehavior>().LevelMilestones[Player.GetComponent<PlayerBehavior>().xpToLevels(CurrentXp)]);
        }

        if (SkillUpCount > 0)
        {
            LevelChoicesScreen.GetComponent<LevelManager>().numOfSkilUps = SkillUpCount;
            LevelChoicesScreen.gameObject.SetActive(true);
        }

        this.transform.GetChild(0).GetComponent<Button>().interactable = true;
        this.transform.GetChild(3).GetComponent<Button>().interactable = true;
        this.transform.GetChild(2).GetComponent<Image>().color = this.transform.GetChild(3).GetComponent<Button>().colors.normalColor;
    }

    public void closeEndofDay()
    {
        if(!LevelChoicesScreen.gameObject.activeSelf)
        {
            Player.GetComponent<GameManager>().start();
            Player.GetComponent<GameManager>().BlackBackground.gameObject.SetActive(false);
        }
        this.transform.GetChild(0).GetComponent<Button>().interactable = false;
        this.transform.GetChild(3).GetComponent<Button>().interactable = false;
        this.transform.GetChild(2).GetComponent<Image>().color = this.transform.GetChild(3).GetComponent<Button>().colors.disabledColor;
        this.transform.GetChild(4).GetChild(2).gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
