using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<Transform> Skills;
    public List<Transform> AvaliableSkills;
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
        Calls.Add("Quick Movement", QuickMovement);
        Calls.Add("Enhanced Strength", EnhancedStrength);
        Calls.Add("Advanced Cooking", AdvancedCooking);
        Calls.Add("Enhanced Movement", EnhancedMovement);
        Calls.Add("Customer Preference - Antinium", CustomerPreferenceAntinium);
        Calls.Add("Customer Preference - Goblin", CustomerPreferenceGoblin);
        Calls.Add("Customer Preference - Drake", CustomerPreferenceDrake);
        Calls.Add("Inn - Calming Presence", InnCalmingPresence);
        Calls.Add("Magnified Training", MagnifiedTraining);
        Calls.Add("One More Portion", OneMorePortion);
        Calls.Add("Basic Preparation", BasicPreparation);
        Calls.Add("Advanced Preparation", AdvancedPreparation);
        Calls.Add("Basic Gathering", BasicGathering);
        Calls.Add("Advanced Gathering", AdvancedGathering);
        Calls.Add("Inn - Lethargic Steps", InnLethargicSteps);
        Calls.Add("Discount Runner", DiscountRunner);
        Calls.Add("Fancy Food", FancyFood);
        Calls.Add("Proficiency - Haggling", ProficiencyHaggling);
        Calls.Add("Local Landmark - Liscor", LocalLandmarkLiscor);
        Calls.Add("Inn - Generous Tippers", InnGenerousTippers);
        Calls.Add("Field of Preservation", FieldOfPreservation);
        Calls.Add("Ready To Cook", ReadyToCook);
        Calls.Add("Fill Container - Water", FillContainerWater);
        Calls.Add("Quick Boiling", QuickBoiling);
        Calls.Add("Any Meal Will Do", AnyMealWillDo);
        Calls.Add("Inn, My Hand", InnMyHand);

        SkillDictionary = new Dictionary<string, Transform>();
        foreach(Transform skill in Skills)
        {
            SkillDictionary.Add(skill.name, skill);
        }
        /*
        AvaliableSkills.Add(SkillDictionary["Lesser Strength"]);
        AvaliableSkills.Add(SkillDictionary["Basic Cooking"]);
        AvaliableSkills.Add(SkillDictionary["Quick Movement"]);
        AvaliableSkills.Add(SkillDictionary["Basic Chopping"]);
        AvaliableSkills.Add(SkillDictionary["Basic Gathering"]);
        AvaliableSkills.Add(SkillDictionary["Inn - Lethargic Steps"]);
        AvaliableSkills.Add(SkillDictionary["Magnified Training"]);
        AvaliableSkills.Add(SkillDictionary["Extra Portion"]);
        AvaliableSkills.Add(SkillDictionary["Customer Preference - Antinium"]);
        AvaliableSkills.Add(SkillDictionary["Customer Preference - Goblin"]);
        AvaliableSkills.Add(SkillDictionary["Customer Preference - Drake"]);
        AvaliableSkills.Add(SkillDictionary["Inn - Calming Presence"]);
        AvaliableSkills.Add(SkillDictionary["Discount Runner"]);
        AvaliableSkills.Add(SkillDictionary["Fancy Food"]);
        AvaliableSkills.Add(SkillDictionary["Proficiency - Haggling"]);
        AvaliableSkills.Add(SkillDictionary["Local Landmark - Liscor"]);
        AvaliableSkills.Add(SkillDictionary["Generous Tippers"]);*/
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
        foreach (Transform skill in AvaliableSkills)
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
                if (skills.Values[0].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[0].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[0].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[0].name);
                }
                skills.RemoveAt(0);
            }
            else if (choice < 45)
            {
                int index = 1;
                if (skills.Count < 2)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else if (choice < 60)
            {
                int index = 2;
                if (skills.Count < 3)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else if (choice < 72)
            {
                int index = 3;
                if (skills.Count < 4)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else if (choice < 80)
            {
                int index = 4;
                if (skills.Count < 5)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else if (choice < 86)
            {
                int index = 5;
                if (skills.Count < 6)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else if (choice < 91)
            {
                int index = 6;
                if (skills.Count < 7)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else if (choice < 95)
            {
                int index = 7;
                if (skills.Count < 8)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else if (choice < 98)
            {
                int index = 8;
                if (skills.Count < 9)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
            }
            else
            {
                int index = 9;
                if (skills.Count < 10)
                {
                    index = skills.Count - 1;
                }
                if (skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    Skillchoice.Add("Skill Change - " + skills.Values[index].GetComponent<SkillBehavior>().PrerequisiteSkill + " - " + skills.Values[index].name);
                }
                else
                {
                    Skillchoice.Add(skills.Values[index].name);
                }
                skills.RemoveAt(index);
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
                if(skill.GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    btn1.transform.GetChild(0).GetComponent<Text>().text = skill.GetComponent<SkillBehavior>().PrerequisiteSkill.name + " -> " + Skill1;
                }
                else
                {
                    btn1.transform.GetChild(0).GetComponent<Text>().text = Skill1;
                }
                break;
            }
        }
        
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
                if (skill.GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    btn2.transform.GetChild(0).GetComponent<Text>().text = skill.GetComponent<SkillBehavior>().PrerequisiteSkill.name + " -> " + Skill2;
                }
                else
                {
                    btn2.transform.GetChild(0).GetComponent<Text>().text = Skill2;
                }
                break;
            }
        }

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
                if (skill.GetComponent<SkillBehavior>().PrerequisiteSkill != null)
                {
                    btn3.transform.GetChild(0).GetComponent<Text>().text = skill.GetComponent<SkillBehavior>().PrerequisiteSkill.name + " -> " + Skill3;
                }
                else
                {
                    btn3.transform.GetChild(0).GetComponent<Text>().text = Skill3;
                }
                break;
            }
        }

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
                GameObject.Find("Market Screen").transform.GetChild(3).GetComponent<Button>().interactable = true;
            }
            this.gameObject.SetActive(false);
        }
    }


    void LesserStrength()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if(!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Strength"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().strength += .1f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Lesser Strength");
            AvaliableSkills.Remove(SkillDictionary["Lesser Strength"]);
            AvaliableSkills.Add(SkillDictionary["Enhanced Strength"]);
        }
        Player.GetComponent<PlayerBehavior>().strength += (currentLevel - pastLevel) * .01f;
        CheckForMoreSkills();
    }

    private void BasicCooking()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Cooking"))
        {
            pastLevel = 0;
            Player.GetComponent<ResourceManager>().CookingTimeDelay -= 2.5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Cooking");
            AvaliableSkills.Remove(SkillDictionary["Basic Cooking"]);
            AvaliableSkills.Add(SkillDictionary["Advanced Cooking"]);
        }
        Player.GetComponent<ResourceManager>().CookingTimeDelay -= (currentLevel - pastLevel) * .25f;
        CheckForMoreSkills();
    }

    private void QuickMovement()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Quick Movement"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().MovementSpeed += .5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Quick Movement");
            AvaliableSkills.Remove(SkillDictionary["Quick Movement"]);
            AvaliableSkills.Add(SkillDictionary["Enhanced Movement"]);
        }
        Player.GetComponent<PlayerBehavior>().MovementSpeed += (currentLevel - pastLevel) * .025f;
        CheckForMoreSkills();
    }

    void EnhancedStrength()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Strength"))
        {
            Player.GetComponent<PlayerBehavior>().strength += ((currentLevel - pastLevel) * .01f);
            if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Strength"))
            {
                Player.GetComponent<PlayerBehavior>().PlayerSkills.Remove("Lesser Strength");
                AvaliableSkills.Remove(SkillDictionary["Enhanced Strength"]);
            }
            else
            {
                AvaliableSkills.Remove(SkillDictionary["Lesser Strength"]);
            }

            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().strength += .1f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Strength");
            Player.GetComponent<PlayerBehavior>().strength += ((currentLevel - pastLevel) * .01f);
        }
        else
        {
            Player.GetComponent<PlayerBehavior>().strength += (currentLevel - pastLevel) * .02f;
        }
        CheckForMoreSkills();
    }

    private void AdvancedCooking()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Cooking"))
        {
            Player.GetComponent<ResourceManager>().CookingTimeDelay -= ((currentLevel - pastLevel) * .25f);
            if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Cooking"))
            {
                Player.GetComponent<PlayerBehavior>().PlayerSkills.Remove("Basic Cooking");
                AvaliableSkills.Remove(SkillDictionary["Advanced Cooking"]);
            }
            else
            {
                AvaliableSkills.Remove(SkillDictionary["Basic Cooking"]);
            }
            pastLevel = 0;
            Player.GetComponent<ResourceManager>().CookingTimeDelay -= 2.5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Cooking");
            Player.GetComponent<ResourceManager>().CookingTimeDelay -= ((currentLevel - pastLevel) * .25f);
        }
        else
        {
            Player.GetComponent<ResourceManager>().CookingTimeDelay -= (currentLevel - pastLevel) * .5f;
        }
        CheckForMoreSkills();
    }

    private void EnhancedMovement()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Movement"))
        {
            Player.GetComponent<PlayerBehavior>().MovementSpeed += ((currentLevel - pastLevel) * .025f);
            if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Quick Movement"))
            {
                Player.GetComponent<PlayerBehavior>().PlayerSkills.Remove("Quick Movement");
                AvaliableSkills.Remove(SkillDictionary["Enhanced Movement"]);
            }
            else
            {
                AvaliableSkills.Remove(SkillDictionary["Quick Movement"]);
            }
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().MovementSpeed += .5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Movement");
            Player.GetComponent<PlayerBehavior>().MovementSpeed += ((currentLevel - pastLevel) * .025f);
        }
        else
        {
            Player.GetComponent<PlayerBehavior>().MovementSpeed += (currentLevel - pastLevel) * .05f;
        }
        CheckForMoreSkills();
    }

    private void OneMorePortion()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("One More Portion"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("One More Portion");
            AvaliableSkills.Remove(SkillDictionary["One More Portion"]);
        }
        int extraGain = (currentLevel / 5) - (pastLevel / 5);
        Player.GetComponent<ResourceManager>().BlueFruitJuiceGain += extraGain; //number of Blue Fruit Juice created with each create action
        Player.GetComponent<GameManager>().CraftingPopup.GetChild(5).GetComponent<Info>().Description = 
            "<b>Blue Fruit Juice</b> - Juice <color=#F7D64A>(1)</color> blue fruit into <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().BlueFruitJuiceGain + 
            ")</color> glasses of water to create <color=#F7D64A>(3)</color> glasses of blue fruit juice.";
        Player.GetComponent<ResourceManager>().SlicedBlueFruitGain += extraGain; //number of Sliced Blue Fruit created with each create action
        Player.GetComponent<GameManager>().CraftingPopup.GetChild(4).GetComponent<Info>().Description =
            "<b>Slice Blue Fruit</b> - Slice <color=#F7D64A>(1)</color> Blue Fruit into <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().SlicedBlueFruitGain +
            ")</color> Meals of Sliced Blue Fruit";
        Player.GetComponent<ResourceManager>().PastaGain += extraGain; //number of Patsa created with each create action
        foreach(Transform cauldronPopup in Player.GetComponent<GameManager>().CauldronPopups)
        {
            cauldronPopup.GetChild(3).GetComponent<Info>().Description =
            "<b>Boil some pasta</b> - Use <color=#F7D64A>(3)</color> Dried Noodles from your hand to boil it into <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().PastaGain +
            ")</color> Bowls of Pasta.";
        }
        Player.GetComponent<ResourceManager>().WaterGlassGain += extraGain; //number of Water created with each create action
        foreach (Transform cauldronPopup in Player.GetComponent<GameManager>().CauldronPopups)
        {
            cauldronPopup.GetChild(4).GetComponent<Info>().Description =
            "<b>Glass of Water</b> - <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().WaterGlassGain +
            ")</color> Glasses of Boiled Water collected and heated in your cauldron. Standard stuff.";
        }
        Player.GetComponent<ResourceManager>().DeAcidFlyGain += extraGain;
        Player.GetComponent<GameManager>().CraftingPopup.GetChild(3).GetComponent<Info>().Description =
            "<b>Seperate Acid from Flies</b> - Violently shake <color=#F7D64A>(1)</color> Jar of Acid Flies to kill the fragile creatures. Then seperate the fly corpses from the acid and place them into <color=#F7D64A>(" +
            Player.GetComponent<ResourceManager>().DeAcidFlyGain + ")</color> bowls.";
        CheckForMoreSkills();
    }

    private void CustomerPreferenceDrake()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Drake"))
        {
            pastLevel = 0;
            Customer.GetComponent<CustomerBehavior>().DrakeChance += 20;
            if (AvaliableSkills.Contains(SkillDictionary["Customer Preference - Goblin"]))
            {
                Customer.GetComponent<CustomerBehavior>().GoblinChance -= 10;
                AvaliableSkills.Remove(SkillDictionary["Customer Preference - Goblin"]);
            }
            Customer.GetComponent<CustomerBehavior>().AntiniumChance -= 10;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Drake");
            AvaliableSkills.Remove(SkillDictionary["Customer Preference - Drake"]);
            AvaliableSkills.Remove(SkillDictionary["Customer Preference - Antinium"]);
        }
        if (Customer.GetComponent<CustomerBehavior>().GoblinChance == 0)
        {
            Customer.GetComponent<CustomerBehavior>().DrakeChance += (currentLevel - pastLevel) * 2;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance += -(currentLevel - pastLevel) * 2;
        }
        else
        {
            Customer.GetComponent<CustomerBehavior>().DrakeChance += (currentLevel - pastLevel) * 2;
            Customer.GetComponent<CustomerBehavior>().GoblinChance += -(currentLevel - pastLevel) * 1;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance += -(currentLevel - pastLevel) * 1;
        }
        CheckForMoreSkills();
    }

    private void CustomerPreferenceGoblin()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Goblin"))
        {
            pastLevel = 0;
            Customer.GetComponent<CustomerBehavior>().DrakeChance -= 10;
            Customer.GetComponent<CustomerBehavior>().GoblinChance += 20;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance -= 10;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Goblin");
            AvaliableSkills.Remove(SkillDictionary["Customer Preference - Drake"]);
            AvaliableSkills.Remove(SkillDictionary["Customer Preference - Goblin"]);
            AvaliableSkills.Remove(SkillDictionary["Customer Preference - Antinium"]);
        }
        Customer.GetComponent<CustomerBehavior>().DrakeChance += -(currentLevel - pastLevel) * 1;
        Customer.GetComponent<CustomerBehavior>().GoblinChance += (currentLevel - pastLevel) * 2;
        Customer.GetComponent<CustomerBehavior>().AntiniumChance += -(currentLevel - pastLevel) * 1;
        CheckForMoreSkills();
    }

    private void CustomerPreferenceAntinium()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Customer Preference - Antinium"))
        {
            pastLevel = 0;
            Customer.GetComponent<CustomerBehavior>().DrakeChance -= 10;
            if (AvaliableSkills.Contains(SkillDictionary["Customer Preference - Goblin"]))
            {
                Customer.GetComponent<CustomerBehavior>().GoblinChance -= 10;
                AvaliableSkills.Remove(SkillDictionary["Customer Preference - Goblin"]);
            }
            Customer.GetComponent<CustomerBehavior>().AntiniumChance += 20;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Customer Preference - Antinium");
            AvaliableSkills.Remove(SkillDictionary["Customer Preference - Drake"]);
            AvaliableSkills.Remove(SkillDictionary["Customer Preference - Antinium"]);
        }
        if (Customer.GetComponent<CustomerBehavior>().GoblinChance == 0)
        {
            Customer.GetComponent<CustomerBehavior>().DrakeChance += -(currentLevel - pastLevel) * 2;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance += (currentLevel - pastLevel) * 2;
        }
        else
        {
            Customer.GetComponent<CustomerBehavior>().DrakeChance += -(currentLevel - pastLevel) * 1;
            Customer.GetComponent<CustomerBehavior>().GoblinChance += -(currentLevel - pastLevel) * 1;
            Customer.GetComponent<CustomerBehavior>().AntiniumChance += (currentLevel - pastLevel) * 2;
        }
        CheckForMoreSkills();
    }

    private void MagnifiedTraining()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Magnified Training"))
        {
            pastLevel = 0;
            CustomerPopup.transform.GetChild(3).GetComponent<CustomerRequestBehavior>().xpGain += 8;
            Player.GetComponent<GameManager>().CustomerSatisfactionXpBonus += 40;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Magnified Training");
            AvaliableSkills.Remove(SkillDictionary["Magnified Training"]);
        }
        CustomerPopup.transform.GetChild(3).GetComponent<CustomerRequestBehavior>().xpGain += (currentLevel - pastLevel) * 1;
        Player.GetComponent<GameManager>().CustomerSatisfactionXpBonus += (currentLevel - pastLevel) * 5;
        CheckForMoreSkills();
    }

    private void InnCalmingPresence()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn - Calming Presence"))
        {
            pastLevel = 0;
            Customer.GetComponent<CustomerBehavior>().LifeTimer += 20;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn - Calming Presence");
            AvaliableSkills.Remove(SkillDictionary["Inn - Calming Presence"]);
        }
        Customer.GetComponent<CustomerBehavior>().LifeTimer += (currentLevel - pastLevel) * 2;
        CheckForMoreSkills();
    }

    private void BasicPreparation()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Preparation"))
        {
            pastLevel = 0;
            Player.GetComponent<ResourceManager>().CraftingTimeDelay -= 1.5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Preparation");
            AvaliableSkills.Remove(SkillDictionary["Basic Preparation"]);
            AvaliableSkills.Add(SkillDictionary["Advanced Preparation"]);
        }
        Player.GetComponent<ResourceManager>().CraftingTimeDelay -= ((currentLevel - pastLevel) * .1f);
        CheckForMoreSkills();
    }

    private void AdvancedPreparation()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Preparation"))
        {
            Player.GetComponent<ResourceManager>().CraftingTimeDelay -= ((currentLevel - pastLevel) * .1f);
            if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Preparation"))
            {
                Player.GetComponent<PlayerBehavior>().PlayerSkills.Remove("Basic Preparation");
                AvaliableSkills.Remove(SkillDictionary["Advanced Preparation"]);
            }
            else
            {
                AvaliableSkills.Remove(SkillDictionary["Basic Preparation"]);
            }
            pastLevel = 0;
            Player.GetComponent<ResourceManager>().CraftingTimeDelay -= 1.5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Preparation");
            Player.GetComponent<ResourceManager>().CraftingTimeDelay -= ((currentLevel - pastLevel) * .1f);
        }
        else
        {
            Player.GetComponent<ResourceManager>().CraftingTimeDelay -= ((currentLevel - pastLevel) * .2f);
        }
        CheckForMoreSkills();
    }

    private void BasicGathering()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Gathering"))
        {
            pastLevel = 0;
            Player.GetComponent<ResourceManager>().GatheringTimeDelay -= 1;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Gathering");
            AvaliableSkills.Remove(SkillDictionary["Basic Gathering"]);
            AvaliableSkills.Add(SkillDictionary["Advanced Gathering"]);
        }
        Player.GetComponent<ResourceManager>().GatheringTimeDelay -= ((currentLevel - pastLevel) * .05f);
        CheckForMoreSkills();
    }

    private void AdvancedGathering()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Gathering"))
        {
            Player.GetComponent<ResourceManager>().GatheringTimeDelay -= ((currentLevel - pastLevel) * .05f);
            if (Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Gathering"))
            {
                Player.GetComponent<PlayerBehavior>().PlayerSkills.Remove("Basic Gathering");
                AvaliableSkills.Remove(SkillDictionary["Advanced Gathering"]);
            }
            else
            {
                AvaliableSkills.Remove(SkillDictionary["Basic Gathering"]);
            }
            pastLevel = 0;
            Player.GetComponent<ResourceManager>().GatheringTimeDelay -= 1;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Gathering");
            Player.GetComponent<ResourceManager>().GatheringTimeDelay -= ((currentLevel - pastLevel) * .05f);
        }
        else
        {
            Player.GetComponent<ResourceManager>().GatheringTimeDelay -= ((currentLevel - pastLevel) * .1f);
        }
        CheckForMoreSkills();
    }

    private void InnLethargicSteps()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn - Lethargic Steps"))
        {
            pastLevel = 0;
            Customer.GetComponent<CustomerBehavior>().MovementSpeed -= 5f;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn - Lethargic Steps");
            AvaliableSkills.Remove(SkillDictionary["Inn - Lethargic Steps"]);
        }
        Customer.GetComponent<CustomerBehavior>().MovementSpeed -= ((currentLevel - pastLevel) * .5f);
        CheckForMoreSkills();
    }

    private void DiscountRunner()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Discount Runner"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Discount Runner");
            AvaliableSkills.Remove(SkillDictionary["Discount Runner"]);
        }
        int discount = (currentLevel / 5) - (pastLevel / 5);
        Player.GetComponent<ResourceManager>().BlueFruit.GetComponent<ItemBehavior>().ItemValue = 
            Mathf.Max(1, Player.GetComponent<ResourceManager>().BlueFruit.GetComponent<ItemBehavior>().ItemValue - discount);
        Player.GetComponent<GameManager>().PurchaseBoxPupup.GetChild(3).GetComponent<Info>().Description = 
            "<b>Buy Blue Fruits</b> - Buy a Blue Fruit for <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().BlueFruit.GetComponent<ItemBehavior>().ItemValue + ")</color> copper coin(s).";
        Player.GetComponent<ResourceManager>().Water.GetComponent<ItemBehavior>().ItemValue =
            Mathf.Max(1, Player.GetComponent<ResourceManager>().Water.GetComponent<ItemBehavior>().ItemValue - discount);
        Player.GetComponent<GameManager>().PurchaseBoxPupup.GetChild(4).GetComponent<Info>().Description =
            "<b>Buy Water</b> - Buy a bucket of Water for <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().Water.GetComponent<ItemBehavior>().ItemValue + ")</color> copper coin(s).";
        Player.GetComponent<ResourceManager>().Noodle.GetComponent<ItemBehavior>().ItemValue =
            Mathf.Max(1, Player.GetComponent<ResourceManager>().Noodle.GetComponent<ItemBehavior>().ItemValue - discount);
        Player.GetComponent<GameManager>().PurchaseBoxPupup.GetChild(5).GetComponent<Info>().Description =
            "<b>Buy Acid Flys</b> - Buy a jar of captured Acid Flys for <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().Noodle.GetComponent<ItemBehavior>().ItemValue + ")</color> copper coin(s).";
        Player.GetComponent<ResourceManager>().AcidFly.GetComponent<ItemBehavior>().ItemValue =
            Mathf.Max(1, Player.GetComponent<ResourceManager>().AcidFly.GetComponent<ItemBehavior>().ItemValue - discount);
        Player.GetComponent<GameManager>().PurchaseBoxPupup.GetChild(6).GetComponent<Info>().Description =
            "<b>Buy Dried Noodles</b> - Buy Dried Noodle for <color=#F7D64A>(" + Player.GetComponent<ResourceManager>().AcidFly.GetComponent<ItemBehavior>().ItemValue + ")</color> copper coin(s).";
        CheckForMoreSkills();
    }

    private void FancyFood()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Fancy Food"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Fancy Food");
            AvaliableSkills.Remove(SkillDictionary["Fancy Food"]);
        }
        int markup = (currentLevel / 5) - (pastLevel / 5);
        Player.GetComponent<ResourceManager>().BlueFruitSlice.GetComponent<ItemBehavior>().ItemValue = Player.GetComponent<ResourceManager>().BlueFruitSlice.GetComponent<ItemBehavior>().ItemValue + markup;
        Player.GetComponent<ResourceManager>().BlueFruitJuice.GetComponent<ItemBehavior>().ItemValue = Player.GetComponent<ResourceManager>().BlueFruitJuice.GetComponent<ItemBehavior>().ItemValue + markup;
        Player.GetComponent<ResourceManager>().GlassWater.GetComponent<ItemBehavior>().ItemValue = Player.GetComponent<ResourceManager>().GlassWater.GetComponent<ItemBehavior>().ItemValue + markup;
        Player.GetComponent<ResourceManager>().PastaBowl.GetComponent<ItemBehavior>().ItemValue = Player.GetComponent<ResourceManager>().PastaBowl.GetComponent<ItemBehavior>().ItemValue + markup;
        Player.GetComponent<ResourceManager>().DeAcidFly.GetComponent<ItemBehavior>().ItemValue = Player.GetComponent<ResourceManager>().DeAcidFly.GetComponent<ItemBehavior>().ItemValue + markup;
        CheckForMoreSkills();
    }

    private void ProficiencyHaggling()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Proficiency - Haggling"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Proficiency - Haggling");
            AvaliableSkills.Remove(SkillDictionary["Proficiency - Haggling"]);
        }
        int discount = (currentLevel / 2) - (pastLevel / 2);
        Transform content = Player.GetComponent<GameManager>().MarketScreen.transform.GetChild(1).GetChild(0).GetChild(0);
        for(int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).GetChild(1).GetComponent<PurchaseInfo>().Cost = Mathf.Max(1, content.GetChild(i).GetChild(1).GetComponent<PurchaseInfo>().Cost - discount);
            content.GetChild(i).GetChild(1).GetComponent<PurchaseInfo>().updateCost();
        }
        CheckForMoreSkills();
    }

    private void LocalLandmarkLiscor()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Local Landmark - Liscor"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Local Landmark - Liscor");
            AvaliableSkills.Remove(SkillDictionary["Local Landmark - Liscor"]);
        }
        for(int i = (currentLevel / 5) - (pastLevel / 5); i > 0; i--)
        {
            Player.GetComponent<GameManager>().createDisSatisfiedCustomerCounters();
        }
        CheckForMoreSkills();
    }

    private void InnGenerousTippers()
    {
        int currentLevel = Player.GetComponent<PlayerBehavior>().Level;
        int pastLevel = Player.GetComponent<PlayerBehavior>().xpToLevels(Player.GetComponent<PlayerBehavior>().PreviousXp);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn - Generous Tippers"))
        {
            pastLevel = 0;
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn - Generous Tippers");
            AvaliableSkills.Remove(SkillDictionary["Inn - Generous Tippers"]);
        }
        CustomerPopup.transform.GetChild(3).GetComponent<CustomerRequestBehavior>().maxTip += (currentLevel / 2) - (pastLevel / 2);
        CheckForMoreSkills();
    }

    private void FieldOfPreservation()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Field of Preservation"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Field of Preservation");
            AvaliableSkills.Remove(SkillDictionary["Field of Preservation"]);
        }
        CheckForMoreSkills();
    }

    private void ReadyToCook()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Ready To Cook"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Ready To Cook");
            AvaliableSkills.Remove(SkillDictionary["Ready To Cook"]);
        }
        CheckForMoreSkills();
    }

    private void StayAwhile()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Stay Awhile"))
        {
            Customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).gameObject.SetActive(true);
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Stay Awhile");
            AvaliableSkills.Remove(SkillDictionary["Stay Awhile"]);
        }
        CheckForMoreSkills();
    }

    private void DinerDash()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Diner Dash"))
        {
            Player.GetComponent<GameManager>().DashIndicator.gameObject.SetActive(true);
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Diner Dash");
            AvaliableSkills.Remove(SkillDictionary["Diner Dash"]);
        }
        Player.GetComponent<PlayerBehavior>().canDash = true;
        this.GetComponent<GameManager>().DashIndicator.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        CheckForMoreSkills();
    }

    private void FillContainerWater()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Fill Container - Water"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Fill Container - Water");
            AvaliableSkills.Remove(SkillDictionary["Fill Container - Water"]);
            foreach(Transform popup in Player.GetComponent<GameManager>().CauldronPopups)
            {
                popup.GetChild(5).gameObject.SetActive(true);
            }
        }
        CheckForMoreSkills();
    }

    private void QuickBoiling()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Quick Boiling"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Quick Boiling");
            AvaliableSkills.Remove(SkillDictionary["Quick Boiling"]);
            foreach (Transform popup in Player.GetComponent<GameManager>().CauldronPopups)
            {
                popup.GetChild(6).gameObject.SetActive(true);
            }
        }
        CheckForMoreSkills();
    }

    private void AnyMealWillDo()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Any Meal Will Do"))
        {
            Player.GetComponent<GameManager>().AnyMealWillDoIndicator.gameObject.SetActive(true);
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Any Meal Will Do");
            AvaliableSkills.Remove(SkillDictionary["Any Meal Will Do"]);
        }
        Player.GetComponent<GameManager>().canAnyMeal = true;
        Player.GetComponent<GameManager>().AnyMealWillDoIndicator.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        CheckForMoreSkills();
    }

    private void InnMyHand()
    {
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn, My Hand"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn, My Hand");
            AvaliableSkills.Remove(SkillDictionary["Inn, My Hand"]);
        }
        CheckForMoreSkills();
    }
}