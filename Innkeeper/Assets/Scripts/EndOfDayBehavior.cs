using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfDayBehavior : MonoBehaviour
{
    public Transform LevelChoicesScreen;
    public Transform MarketScreen;

    private PlayerBehavior Player;

    
    // Start is called before the first frame update
    void Start()
    {
        if(LevelChoicesScreen == null)
        {
            Debug.LogError(name + " could not find Level Up Choice Selection Screen on Startup");
        }
        if (MarketScreen == null)
        {
            Debug.LogError(name + " could not find Market Screen on Startup");
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
        this.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = "0";

        if (Player == null)
        {
            Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        }

        yield return new WaitForSeconds(1.5f);

        if (SatisfiedCustomers > 0)
        {
            this.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = SatisfiedCustomers + "";
            yield return new WaitForSeconds(1f);
        }

        if (DisSatisfiedCustomers > 0)
        {
            this.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = DisSatisfiedCustomers + "";
            yield return new WaitForSeconds(1f);
        }

        if (Player.Level != 10)
        {
            int SkillUpCount = 0;
            while (Player.xpToLevels(CurrentXp) > Player.xpToLevels(PreviousXp))
            {
                this.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().setSizeByPercentage(1);
                while (this.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<Animation>().isPlaying)
                {
                    yield return new WaitForEndOfFrame();
                }
                PreviousXp = Player.LevelMilestones[Player.xpToLevels(PreviousXp)];
                this.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text =
                "Level " + (int.Parse(this.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text.Substring(6)) + 1) + "";
                if ((int.Parse(this.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text.Substring(6))) % 2 == 0)
                {
                    SkillUpCount++;
                }
            }
            if (CurrentXp != PreviousXp)
            {
                this.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().setSizeByPercentage((CurrentXp - PreviousXp) / Player.LevelMilestones[Player.xpToLevels(CurrentXp)]);
                while (this.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<Animation>().isPlaying)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            if (SkillUpCount > 0)
            {
                LevelChoicesScreen.GetComponent<LevelManager>().numOfSkilUps = SkillUpCount;
                LevelChoicesScreen.gameObject.SetActive(true);
                MarketScreen.transform.GetChild(3).GetComponent<Button>().interactable = false;
            }
        }

        this.transform.GetChild(2).GetComponent<Button>().interactable = true;
    }
}
