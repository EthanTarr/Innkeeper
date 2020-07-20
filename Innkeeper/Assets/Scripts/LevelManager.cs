using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<Transform> Skills;
    [HideInInspector] public Dictionary<string, Transform> SkillDictionary;

    public Transform Player;
    public Transform Customer;
    public GameObject CustomerPopup;
    public Transform Tooltip;

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
        Calls.Add("Inn - Lethargic Steps", InnLethargicSteps);

        SkillDictionary = new Dictionary<string, Transform>();
        foreach(Transform skill in Skills)
        {
            SkillDictionary.Add(skill.name, skill);
        }
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
            if (skill.GetComponent<SkillBehavior>().Level - Player.GetComponent<PlayerBehavior>().Level > -3 && !Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains(skill.name))
            {
                float skillValue = skill.GetComponent<SkillBehavior>().Base + (skill.GetComponent<SkillBehavior>().Level - Player.GetComponent<PlayerBehavior>().Level) * 10 -
                skill.GetComponent<SkillBehavior>().Modifier;
                while (skills.ContainsKey(skillValue))
                {
                    skillValue++;
                }
                skills.Add(skillValue, skill);
            }
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
        btn1.onClick.RemoveAllListeners();
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
        btn2.onClick.RemoveAllListeners();
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
        btn3.onClick.RemoveAllListeners();
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
        Tooltip.gameObject.SetActive(false);
        numOfSkilUps--;
        if (numOfSkilUps > 0)
        {
            getChoices();
        } 
        else
        {
            if (!Player.GetComponent<GameManager>().UnlockedFoodScreen.gameObject.activeSelf && this.gameObject.activeSelf)
            {
                Player.GetComponent<GameManager>().start();
                Player.GetComponent<GameManager>().BlackBackground.gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }
    }


    void LesserStrength()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<PlayerBehavior>().strength += (currentLevel - pastLevel) * .01f;
        if(!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Strength"))
        {
            Player.GetComponent<PlayerBehavior>().strength += .1f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Lesser Strength");
            Debug.Log("Lesser Strength");
        }
        CheckForMoreSkills();
    }

    private void BasicCooking()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<ResourceManager>().CookingTimeDelay -= (currentLevel - pastLevel) * .25f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Cooking"))
        {
            Player.GetComponent<ResourceManager>().CookingTimeDelay -= 2.5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Cooking");
            Debug.Log("Basic Cooking");
        }
        CheckForMoreSkills();
    }

    private void LesserSpeed()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<PlayerBehavior>().MovementSpeed += (currentLevel - pastLevel) * .025f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Speed"))
        {
            Player.GetComponent<PlayerBehavior>().MovementSpeed += .5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Lesser Speed");
            Debug.Log("Lesser Speed");
        }
        CheckForMoreSkills();
    }

    void EnhancedStrength()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<PlayerBehavior>().strength += (currentLevel - pastLevel) * .02f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Strength"))
        {
            Player.GetComponent<PlayerBehavior>().strength += .2f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Strength");
            Debug.Log("Enhanced Strength");
        }
        CheckForMoreSkills();
    }

    private void AdvancedCooking()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<ResourceManager>().CookingTimeDelay -= (currentLevel - pastLevel) * .5f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Cooking"))
        {
            Player.GetComponent<ResourceManager>().CookingTimeDelay -= 5;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Cooking");
            Debug.Log("Advanced Cooking");
        }
        CheckForMoreSkills();
    }

    private void EnhancedSpeed()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<PlayerBehavior>().MovementSpeed += (currentLevel - pastLevel) * .05f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Speed"))
        {
            Player.GetComponent<PlayerBehavior>().MovementSpeed += 1;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Speed");
            Debug.Log("Enhanced Speed");
        }
        CheckForMoreSkills();
    }

    private void ExtraPortion()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        int extraGain = (currentLevel / 5) - (pastLevel / 5);
        Player.GetComponent<ResourceManager>().FruitGain += extraGain; //number of fruits gained with each gather action
        Player.GetComponent<ResourceManager>().WaterGain += extraGain; //number of water gained with each gather action
        Player.GetComponent<ResourceManager>().BlueFruitJuiceGain += extraGain; //number of Blue Fruit Juice created with each create action
        Player.GetComponent<ResourceManager>().SlicedBlueFruitGain += extraGain; //number of Sliced Blue Fruit created with each create action
        Player.GetComponent<ResourceManager>().PastaGain += extraGain; //number of Patsa created with each create action
        Player.GetComponent<ResourceManager>().WaterGlassGain += extraGain; //number of Water created with each create action
        Player.GetComponent<ResourceManager>().NoodleGain += extraGain; //number of Noodles created with each create action
        Player.GetComponent<ResourceManager>().DeAcidFlyGain += extraGain;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Extra Portion"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Extra Portion");
            Debug.Log("Extra Portion");
        }
        CheckForMoreSkills();
    }

    private void CustomerPreferenceDrake()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Customer.GetComponent<CustomerBehavior>().DrakeChance += (currentLevel - pastLevel) * 2;
        Customer.GetComponent<CustomerBehavior>().GoblinChance += -(currentLevel - pastLevel) * 1;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance += -(currentLevel - pastLevel) * 1;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Drake"))
        {
            Customer.GetComponent<CustomerBehavior>().DrakeChance += 20;
            Customer.GetComponent<CustomerBehavior>().GoblinChance -= 10;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance -= 10;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Drake");
            Debug.Log("Customer Preference - Drake");
        }
        CheckForMoreSkills();
    }

    private void CustomerPreferenceGoblin()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Customer.GetComponent<CustomerBehavior>().DrakeChance += -(currentLevel - pastLevel) * 1;
        Customer.GetComponent<CustomerBehavior>().GoblinChance += (currentLevel - pastLevel) * 2;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance += -(currentLevel - pastLevel) * 1;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Goblin"))
        {
            Customer.GetComponent<CustomerBehavior>().DrakeChance -= 10;
            Customer.GetComponent<CustomerBehavior>().GoblinChance += 20;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance -= 10;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Goblin");
            Debug.Log("Customer Preference - Goblin");
        }
        CheckForMoreSkills();
    }

    private void CustomerPreferenceAntinium()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Customer.GetComponent<CustomerBehavior>().DrakeChance += -(currentLevel - pastLevel) * 1;
        Customer.GetComponent<CustomerBehavior>().GoblinChance += -(currentLevel - pastLevel) * 1;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance += (currentLevel - pastLevel) * 2;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Antinium"))
        {
            Customer.GetComponent<CustomerBehavior>().DrakeChance -= 10;
            Customer.GetComponent<CustomerBehavior>().GoblinChance -= 10;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance += 20;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Antinium");
            Debug.Log("Customer Preference - Antinium");
        }
        CheckForMoreSkills();
    }

    private void MagnifiedTraining()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        CustomerPopup.transform.GetChild(3).GetComponent<CustomerRequestBehavior>().xpGain += (currentLevel - pastLevel) * 1;
        Player.GetComponent<GameManager>().CustomerSatisfactionXpBonus += (currentLevel - pastLevel) * 5;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Magnified Training"))
        {
            CustomerPopup.transform.GetChild(3).GetComponent<CustomerRequestBehavior>().xpGain += 10;
            Player.GetComponent<GameManager>().CustomerSatisfactionXpBonus += 50;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Magnified Training");
            Debug.Log("Magnified Training");
        }
        CheckForMoreSkills();
    }

        private void InnCalmingAura()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Customer.GetComponent<CustomerBehavior>().LifeTimer += (currentLevel - pastLevel) * 2;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn - Calming Aura"))
        {
            Customer.GetComponent<CustomerBehavior>().LifeTimer += 20;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn - Calming Aura");
            Debug.Log("Inn - Calming Aura");
        }
        CheckForMoreSkills();
    }

    private void BasicChopping()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<ResourceManager>().CraftingTimeDelay -= ((currentLevel - pastLevel) * .1f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Chopping"))
        {
            Player.GetComponent<ResourceManager>().CraftingTimeDelay -= 1.5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Chopping");
            Debug.Log("Basic Chopping");
        }
        CheckForMoreSkills();
    }

    private void AdvancedChopping()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<ResourceManager>().CraftingTimeDelay -= ((currentLevel - pastLevel) * .2f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Chopping"))
        {
            Player.GetComponent<ResourceManager>().CraftingTimeDelay -= 3;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Chopping");
            Debug.Log("Advanced Chopping");
        }
        CheckForMoreSkills();
    }

    private void BasicGathering()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<ResourceManager>().GatheringTimeDelay -= ((currentLevel - pastLevel) * .05f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Gathering"))
        {
            Player.GetComponent<ResourceManager>().GatheringTimeDelay -= 1;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Gathering");
            Debug.Log("Basic Gathering");
        }
        CheckForMoreSkills();
    }

        private void AdvancedGathering()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Player.GetComponent<ResourceManager>().GatheringTimeDelay -= ((currentLevel - pastLevel) * .1f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Gathering"))
        {
            Player.GetComponent<ResourceManager>().GatheringTimeDelay -= 2;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Gathering");
            Debug.Log("Advanced Gathering");
        }
        CheckForMoreSkills();
    }

    private void InnLethargicSteps()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        Customer.GetComponent<CustomerBehavior>().MovementSpeed -= ((currentLevel - pastLevel) * .05f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn - Lethargic Steps"))
        {
            Customer.GetComponent<CustomerBehavior>().MovementSpeed -= .5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn - Lethargic Steps");
            Debug.Log("Inn - Lethargic Steps");
        }
        CheckForMoreSkills();
    }
}