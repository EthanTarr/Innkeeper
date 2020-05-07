using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<Transform> Skills;

    public Transform Player;
    public Transform Customer;
    public GameObject CustomerPopup;

    [HideInInspector] public Dictionary<string, UnityEngine.Events.UnityAction> Calls;
    [HideInInspector] public int numOfSkilUps = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        Calls = new Dictionary<string, UnityEngine.Events.UnityAction>();
        Calls.Add("Lesser Strength", LesserStrength);
        Calls.Add("Basic Cooking", BasicCooking);
        Calls.Add("Lesser Speed", LesserSpeed);
        Calls.Add("Enhanced Strength", EnhancedStrength);
        Calls.Add("Advanced Cooking", AdvancedCooking);
        Calls.Add("Enhanced Speed", EnhancedSpeed);
        Calls.Add("Customer Preference - Antinium", CustomerPreferenceAntinium);
        Calls.Add("Customer Preference - Goblin", CustomerPreferenceGoblin);
        Calls.Add("Customer Preference - Drake", CustomerPreferenceDrake);
        Calls.Add("Inn - Calming Aura", InnCalmingAura);
        Calls.Add("Magnified Training", MagnifiedTraining);
        Calls.Add("Extra Portion", ExtraPortion);
        Calls.Add("Basic Chopping", BasicChopping);
        Calls.Add("Advanced Chopping", AdvancedChopping);
        Calls.Add("Basic Gathering", BasicGathering);
        Calls.Add("Advanced Gathering", AdvancedGathering);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        getChoices();
    }

    private void getChoices()
    {
        List<string> Skillchoice = new List<string>();
        SortedList<float, Transform> skills = new SortedList<float, Transform>();
        foreach (Transform skill in Skills)
        {
            float skillValue = skill.GetComponent<SkillBehavior>().Base + (skill.GetComponent<SkillBehavior>().Level - Player.GetComponent<PlayerBehavior>().Level) * 10 - 
                skill.GetComponent<SkillBehavior>().Modifier;
            while(skills.ContainsKey(skillValue))
            {
                skillValue++;
            }
            skills.Add(skillValue, skill);
        }
        for (int i = 0; i < 3; i++) {
            float choice = Random.Range(0f, 100f);
            if (choice < 25)
            {
                Skillchoice.Add(skills.Values[0].name);
                skills.RemoveAt(0);
            }
            else if (choice < 45)
            {
                if (skills.Count < 2)
                {
                    Skillchoice.Add(skills.Values[0].name);
                    skills.RemoveAt(0);
                }
                else
                {
                    Skillchoice.Add(skills.Values[1].name);
                    skills.RemoveAt(1);
                }
            }
            else if (choice < 60)
            {
                if (skills.Count < 3)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[2].name);
                    skills.RemoveAt(2);
                }
            }
            else if (choice < 72)
            {
                if (skills.Count < 4)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[3].name);
                    skills.RemoveAt(3);
                }
            }
            else if (choice < 80)
            {
                if (skills.Count < 5)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[4].name);
                    skills.RemoveAt(4);
                }
            }
            else if (choice < 86)
            {
                if (skills.Count < 6)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[5].name);
                    skills.RemoveAt(5);
                }
            }
            else if (choice < 91)
            {
                if (skills.Count < 7)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[6].name);
                    skills.RemoveAt(6);
                }
            }
            else if (choice < 95)
            {
                if (skills.Count < 8)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[7].name);
                    skills.RemoveAt(7);
                }
            }
            else if (choice < 98)
            {
                if (skills.Count < 9)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[8].name);
                    skills.RemoveAt(8);
                }
            }
            else
            {
                if (skills.Count < 10)
                {
                    Skillchoice.Add(skills.Values[skills.Count - 1].name);
                    skills.RemoveAt(skills.Count - 1);
                }
                else
                {
                    Skillchoice.Add(skills.Values[9].name);
                    skills.RemoveAt(9);
                }
            }
            Debug.Log("Random Value: " + choice);
        }
        PopulateChoices(Skillchoice[0], Skillchoice[1], Skillchoice[2]);
    }

    private void PopulateChoices(string Skill1, string Skill2, string Skill3)
    {
        Button btn1 = this.transform.GetChild(1).GetComponent<Button>();
        btn1.onClick.AddListener(Calls[Skill1]);
        foreach(Transform skill in Skills)
        {
            if(skill.name.Equals(Skill1))
            {
                btn1.GetComponent<Info>().Description = skill.GetComponent<SkillBehavior>().Description;
                btn1.GetComponent<Info>().width = skill.GetComponent<SkillBehavior>().PreferedToolTipSize.x;
                btn1.GetComponent<Info>().height = skill.GetComponent<SkillBehavior>().PreferedToolTipSize.y;
            }
        }
        btn1.transform.GetChild(0).GetComponent<Text>().text = Skill1;
        Button btn2 = this.transform.GetChild(3).GetComponent<Button>();
        btn2.onClick.AddListener(Calls[Skill2]);
        foreach (Transform skill in Skills)
        {
            if (skill.name.Equals(Skill2))
            {
                btn2.GetComponent<Info>().Description = skill.GetComponent<SkillBehavior>().Description;
                btn2.GetComponent<Info>().width = skill.GetComponent<SkillBehavior>().PreferedToolTipSize.x;
                btn2.GetComponent<Info>().height = skill.GetComponent<SkillBehavior>().PreferedToolTipSize.y;
            }
        }
        btn2.transform.GetChild(0).GetComponent<Text>().text = Skill2;
        Button btn3 = this.transform.GetChild(5).GetComponent<Button>();
        btn3.onClick.AddListener(Calls[Skill3]);
        foreach (Transform skill in Skills)
        {
            if (skill.name.Equals(Skill3))
            {
                btn3.GetComponent<Info>().Description = skill.GetComponent<SkillBehavior>().Description;
                btn3.GetComponent<Info>().width = skill.GetComponent<SkillBehavior>().PreferedToolTipSize.x;
                btn3.GetComponent<Info>().height = skill.GetComponent<SkillBehavior>().PreferedToolTipSize.y;
            }
        }
        btn3.transform.GetChild(0).GetComponent<Text>().text = Skill3;

    }

    private void CheckForMoreSkills()
    {
        GameObject.Find("Tool Tip").SetActive(false);
        numOfSkilUps--;
        if (numOfSkilUps > 0)
        {
            getChoices();
        } 
        else
        {
            Player.GetComponent<GameManager>().start();
            Player.GetComponent<GameManager>().BlackBackground.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    void LesserStrength()
    {
        Player.GetComponent<PlayerBehavior>().strength = Player.GetComponent<PlayerBehavior>().Level * .005f;
        if(!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Strength"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Lesser Strength");
        }
        CheckForMoreSkills();
    }

    private void BasicCooking()
    {
        Player.GetComponent<ResourceManager>().CookingTimeDelay = 10 - (Player.GetComponent<PlayerBehavior>().Level * .1f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Cooking"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Cooking");
        }
        CheckForMoreSkills();
    }

    private void LesserSpeed()
    {
        Player.GetComponent<PlayerBehavior>().MovementSpeed = 1.5f + Player.GetComponent<PlayerBehavior>().Level * .01f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Speed"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Lesser Speed");
        }
        CheckForMoreSkills();
    }

    void EnhancedStrength()
    {
        Player.GetComponent<PlayerBehavior>().strength = Player.GetComponent<PlayerBehavior>().Level * .015f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Strength"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Strength");
        }
        CheckForMoreSkills();
    }

    private void AdvancedCooking()
    {
        Player.GetComponent<ResourceManager>().CookingTimeDelay = 10 - (Player.GetComponent<PlayerBehavior>().Level * .2f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Cooking"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Cooking");
        }
        CheckForMoreSkills();
    }

    private void EnhancedSpeed()
    {
        Player.GetComponent<PlayerBehavior>().MovementSpeed = 1.5f + Player.GetComponent<PlayerBehavior>().Level * .03f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Speed"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Speed");
        }
        CheckForMoreSkills();
    }

    private void ExtraPortion()
    {
        Player.GetComponent<ResourceManager>().FruitGain = 5 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of fruits gained with each gather action
        Player.GetComponent<ResourceManager>().WaterGain = 3 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of water usages gained with each gather action
        Player.GetComponent<ResourceManager>().BlueFruitJuiceGain = 1 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Blue Fruit Juice created with each create action
        Player.GetComponent<ResourceManager>().AcidFlyGain = 10 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Acid Flys created with each create action
        Player.GetComponent<ResourceManager>().SlicedBlueFruitGain = 2 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Sliced Blue Fruit created with each create action
        GameObject.Find("Cauldron").GetComponent<CauldronBehavior>().PastaGain = 3 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Patsa created with each create action
        GameObject.Find("Cauldron").GetComponent<CauldronBehavior>().WaterGain = 5 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Patsa created with each create action
        Player.GetComponent<ResourceManager>().NoodleGain = 5 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Noodles created with each create action
        Player.GetComponent<ResourceManager>().DeAcidFlyGain = 3 + Player.GetComponent<PlayerBehavior>().Level / 5;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Extra Portion"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Extra Portion");
        }
        CheckForMoreSkills();
    }

    private void CustomerPreferenceDrake()
    {
        Customer.GetComponent<CustomerBehavior>().DrakeChance += Player.GetComponent<PlayerBehavior>().Level * 2;
        Customer.GetComponent<CustomerBehavior>().GoblinChance += - Player.GetComponent<PlayerBehavior>().Level;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance += - Player.GetComponent<PlayerBehavior>().Level;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Drake"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Drake");
        }
        CheckForMoreSkills();
    }

    private void CustomerPreferenceGoblin()
    {
        Customer.GetComponent<CustomerBehavior>().DrakeChance += - Player.GetComponent<PlayerBehavior>().Level;
        Customer.GetComponent<CustomerBehavior>().GoblinChance +=  Player.GetComponent<PlayerBehavior>().Level * 2;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance += - Player.GetComponent<PlayerBehavior>().Level;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Goblin"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Goblin");
        }
        CheckForMoreSkills();
    }

    private void CustomerPreferenceAntinium()
    {
        Customer.GetComponent<CustomerBehavior>().DrakeChance += - Player.GetComponent<PlayerBehavior>().Level;
        Customer.GetComponent<CustomerBehavior>().GoblinChance += - Player.GetComponent<PlayerBehavior>().Level;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance += Player.GetComponent<PlayerBehavior>().Level * 2;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Antinium"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Antinium");
        }
        CheckForMoreSkills();
    }

    private void MagnifiedTraining()
    {
        CustomerPopup.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<CustomerRequestBehavior>().xpGain = 50 + Player.GetComponent<PlayerBehavior>().Level * 10;
        Player.GetComponent<GameManager>().CustomerSatisfactionXpBonus = 50 + Player.GetComponent<PlayerBehavior>().Level * 10;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Magnified Training"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Magnified Training");
        }
        CheckForMoreSkills();
    }

    private void InnCalmingAura()
    {
        Customer.GetComponent<CustomerBehavior>().LifeTimer = 60 + Player.GetComponent<PlayerBehavior>().Level * 4;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn - Calming Aura"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn - Calming Aura");
        }
        CheckForMoreSkills();
    }

    private void BasicChopping()
    {
        Player.GetComponent<ResourceManager>().CraftingTimeDelay = 5 - (Player.GetComponent<PlayerBehavior>().Level * .1f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Chopping"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Chopping");
        }
        CheckForMoreSkills();
    }

    private void AdvancedChopping()
    {
        Player.GetComponent<ResourceManager>().CraftingTimeDelay = 5 - (Player.GetComponent<PlayerBehavior>().Level * .2f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Chopping"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Chopping");
        }
        CheckForMoreSkills();
    }

    private void BasicGathering()
    {
        Player.GetComponent<ResourceManager>().GatheringTimeDelay = 3 - (Player.GetComponent<PlayerBehavior>().Level * .1f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Gathering"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Gathering");
        }
        CheckForMoreSkills();
    }

    private void AdvancedGathering()
    {
        Player.GetComponent<ResourceManager>().GatheringTimeDelay = 3 - (Player.GetComponent<PlayerBehavior>().Level * .2f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Gathering"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Gathering");
        }
        CheckForMoreSkills();
    }
}