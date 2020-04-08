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
        SortedList<int, Transform> skills = new SortedList<int, Transform>();
        foreach (Transform skill in Skills)
        {
            int skillValue = skill.GetComponent<SkillBehavior>().Base + (skill.GetComponent<SkillBehavior>().Level - Player.GetComponent<PlayerBehavior>().Level) * 10;
            while(skills.ContainsKey(skillValue))
            {
                skillValue++;
            }
            skills.Add(skillValue, skill);
        }
        for (int i = 0; i < 3; i++) {
            float choice = Random.Range(0, 100);
            if (choice < 25)
            {
                Skillchoice.Add(skills.Values[0].name);
                skills.RemoveAt(0);
            }
            else if (choice < 45)
            {
                Skillchoice.Add(skills.Values[1].name);
                skills.RemoveAt(1);
            }
            else if (choice < 60)
            {
                Skillchoice.Add(skills.Values[2].name);
                skills.RemoveAt(2);
            }
            else if (choice < 70)
            {
                Skillchoice.Add(skills.Values[3].name);
                skills.RemoveAt(3);
            }
            else if (choice < 78)
            {
                Skillchoice.Add(skills.Values[4].name);
                skills.RemoveAt(4);
            }
            else if (choice < 85)
            {
                Skillchoice.Add(skills.Values[5].name);
                skills.RemoveAt(5);
            }
            else if (choice < 90)
            {
                Skillchoice.Add(skills.Values[6].name);
                skills.RemoveAt(6);
            }
            else if (choice < 94)
            {
                Skillchoice.Add(skills.Values[7].name);
                skills.RemoveAt(7);
            }
            else if (choice < 97)
            {
                Skillchoice.Add(skills.Values[8].name);
                skills.RemoveAt(8);
            }
            else if (choice < 99)
            {
                Skillchoice.Add(skills.Values[9].name);
                skills.RemoveAt(9);
            }
            else
            {
                Skillchoice.Add(skills.Values[10].name);
                skills.RemoveAt(10);
            }
            Debug.Log(Skillchoice[i]);
        }
        PopulateChoices(Skillchoice[0], Skillchoice[1], Skillchoice[2]);
    }

    private void PopulateChoices(string Skill1, string Skill2, string Skill3)
    {
        Button btn1 = this.transform.GetChild(0).GetComponent<Button>();
        btn1.onClick.AddListener(Calls[Skill1]);
        btn1.transform.GetChild(0).GetComponent<Text>().text = Skill1;
        Button btn2 = this.transform.GetChild(1).GetComponent<Button>();
        btn2.onClick.AddListener(Calls[Skill2]);
        btn2.transform.GetChild(0).GetComponent<Text>().text = Skill2;
        Button btn3 = this.transform.GetChild(2).GetComponent<Button>();
        btn3.onClick.AddListener(Calls[Skill3]);
        btn3.transform.GetChild(0).GetComponent<Text>().text = Skill3;

    }

    void LesserStrength()
    {
        Player.GetComponent<PlayerBehavior>().strength = Player.GetComponent<PlayerBehavior>().Level * .005f;
        if(!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Strength"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Lesser Strength");
        }
        this.gameObject.SetActive(false);
    }

    private void BasicCooking()
    {
        Player.GetComponent<ResourceManager>().TimeDelay = 5 - (Player.GetComponent<PlayerBehavior>().Level * .1f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Basic Cooking"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Basic Cooking");
        }
        this.gameObject.SetActive(false);
    }

    private void LesserSpeed()
    {
        Player.GetComponent<PlayerBehavior>().MovementSpeed = 60f + Player.GetComponent<PlayerBehavior>().Level * .5f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Lesser Speed"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Lesser Speed");
        }
        this.gameObject.SetActive(false);
    }

    void EnhancedStrength()
    {
        Player.GetComponent<PlayerBehavior>().strength = Player.GetComponent<PlayerBehavior>().Level * .015f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Strength"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Strength");
        }
        this.gameObject.SetActive(false);
    }

    private void AdvancedCooking()
    {
        Player.GetComponent<ResourceManager>().TimeDelay = 5 - (Player.GetComponent<PlayerBehavior>().Level * .2f);
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Advanced Cooking"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Advanced Cooking");
        }
        this.gameObject.SetActive(false);
    }

    private void EnhancedSpeed()
    {
        Player.GetComponent<PlayerBehavior>().MovementSpeed = 60f + Player.GetComponent<PlayerBehavior>().Level * 1f;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Enhanced Speed"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Enhanced Speed");
        }
        this.gameObject.SetActive(false);
    }

    private void ExtraPortion()
    {
        Player.GetComponent<ResourceManager>().FruitGain = 5 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of fruits gained with each gather action
        Player.GetComponent<ResourceManager>().WaterGain = 3 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of water usages gained with each gather action
        Player.GetComponent<ResourceManager>().BlueFruitJuiceGain = 1 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Blue Fruit Juice created with each create action
        Player.GetComponent<ResourceManager>().AcidFlyGain = 10 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Acid Flys created with each create action
        Player.GetComponent<ResourceManager>().SlicedBlueFruitGain = 2 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Sliced Blue Fruit created with each create action
        Player.GetComponent<ResourceManager>().PastaGain = 1 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Patsa created with each create action
        Player.GetComponent<ResourceManager>().NoodleGain = 5 + Player.GetComponent<PlayerBehavior>().Level / 5; //number of Noodles created with each create action
        Player.GetComponent<ResourceManager>().DeAcidFlyGain = 3 + Player.GetComponent<PlayerBehavior>().Level / 5;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Extra Portion"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Extra Portion");
        }
        this.gameObject.SetActive(false);
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
        this.gameObject.SetActive(false);
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
        this.gameObject.SetActive(false);
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
        this.gameObject.SetActive(false);
    }

    private void MagnifiedTraining()
    {
        CustomerPopup.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<CustomerRequestBehavior>().xpGain = 50 + Player.GetComponent<PlayerBehavior>().Level * 10;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Magnified Training"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Magnified Training");
        }
        this.gameObject.SetActive(false);
    }

    private void InnCalmingAura()
    {
        Customer.GetComponent<CustomerBehavior>().LifeTimer = 60 + Player.GetComponent<PlayerBehavior>().Level * 4;
        if (!Player.GetComponent<PlayerBehavior>().PlayerSkills.Contains("Inn - Calming Aura"))
        {
            Player.GetComponent<PlayerBehavior>().PlayerSkills.Add("Inn - Calming Aura");
        }
        this.gameObject.SetActive(false);
    }
}