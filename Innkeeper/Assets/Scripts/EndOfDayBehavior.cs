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
        this.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = "0";

        if (Player == null)
        {
            Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        }

        yield return new WaitForSeconds(.5f);

        if (SatisfiedCustomers > 0)
        {
            this.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = SatisfiedCustomers + "";
            yield return new WaitForSeconds(.5f);
        }

        if (DisSatisfiedCustomers > 0)
        {
            this.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = DisSatisfiedCustomers + "";
            yield return new WaitForSeconds(.5f);
        }

        if (Player.xpToLevels(PreviousXp) != 20)
        {
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
                int levelText = (int.Parse(this.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text.Substring(6)));
                if (levelText % 10 == 0)
                {
                    if(levelText / 10 == 1)
                    {
                        LevelChoicesScreen.GetComponent<LevelManager>().numOfFirstMilestoneSkillUps++;
                        if (Random.value < .1f)
                        {
                            LevelChoicesScreen.GetComponent<LevelManager>().numOfFirstMilestoneSkillUps++;
                        }
                    }
                    else if (levelText / 10 == 2)
                    {
                        LevelChoicesScreen.GetComponent<LevelManager>().numOfSecondMilestoneSkillUps++;
                        if (Random.value < .1f)
                        {
                            LevelChoicesScreen.GetComponent<LevelManager>().numOfSecondMilestoneSkillUps++;
                        }
                    }
                }
                if (levelText / 10 == 0)
                {
                    if (Random.value > 0.5f)
                    {
                        LevelChoicesScreen.GetComponent<LevelManager>().numOfSubTenSkillUps++;
                        if (Random.value < .1f)
                        {
                            LevelChoicesScreen.GetComponent<LevelManager>().numOfSubTenSkillUps++;
                        }
                    }
                }
                else
                {
                    if (Random.value > 0.5f)
                    {
                        LevelChoicesScreen.GetComponent<LevelManager>().numOfSubTwentySkillUps++;
                        if (Random.value < .1f)
                        {
                            LevelChoicesScreen.GetComponent<LevelManager>().numOfSubTwentySkillUps++;
                        }
                    }
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

            this.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(TurnOnMarket);
            if (LevelChoicesScreen.GetComponent<LevelManager>().numOfSubTenSkillUps > 0 || LevelChoicesScreen.GetComponent<LevelManager>().numOfSubTwentySkillUps > 0 || 
                LevelChoicesScreen.GetComponent<LevelManager>().numOfFirstMilestoneSkillUps > 0 || LevelChoicesScreen.GetComponent<LevelManager>().numOfSecondMilestoneSkillUps > 0)
            {
                LevelChoicesScreen.gameObject.SetActive(true);
                this.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            }
            else if(Player.GetComponent<GameManager>().UnlockedFoodScreen.gameObject.activeSelf)
            {
                this.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        this.transform.GetChild(2).GetComponent<Button>().interactable = true;
    }

    private void TurnOnMarket()
    {
        MarketScreen.gameObject.SetActive(true);
    }
}
