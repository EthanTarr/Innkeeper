using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<string> PlayerSkills = new List<string>();
    public List<string> Purchases = new List<string>();
    public float MovementSpeed = 1.5f;
    public float PreviousXp = 0;
    public float xp = 0;
    public int Level = 0;
    public int money = 0;

    public float SpawnTimerIncreaseAmount = .92f;
    public int DayCount = 1;
    public int DayTimeLimit = 80;
    public int DayStartDelay = 15;
    public float xpBarPercentage = 0;
    public float strength = 0;
    public Dictionary<int, string> UnlockableFoods = new Dictionary<int, string>();
    public string BlueFruitGatherDescription = "<b>Gather Blue Fruits</b> - Gather <color=#F7D64A>(3)</color> Blue Fruits from a nearby orchird of trees. They seem edible.";
    public string WaterGatherDescription = "<b>Gather Water</b> - Gather <color=#F7D64A>(1)</color> Bucket of Water from the nearby stream.";
    public string AcidFlyGatherDescription = "<b>Gather Acid Flys</b> - Gather <color=#F7D64A>(1)</color> Jar of captured Acid Flys.";
    public string NoodleGatherDescription = "<b>Gather Noodle Ingredients</b> - Gather the ingredients for <color=#F7D64A>(3)</color> Noodles from the nearby area.";
    public string DeAcidFlyCreationDescription = "<b>Seperate Acid from Flies</b> - Violently shake <color=#F7D64A>(1)</color> Jar of Acid Flies to kill the fragile creatures. Then seperate the fly corpses from the acid and place them into <color=#F7D64A>(5)</color> bowls.";
    public string SliceBlueFruitCreationDescription = "<b>Slice Blue Fruit</b> - Slice <color=#F7D64A>(1)</color> Blue Fruit into <color=#F7D64A>(2)</color> Meals of Sliced Blue Fruit";
    public string BlueFruitJuiceCreationDescription = "<b>Blue Fruit Juice</b> - Juice <color=#F7D64A>(1)</color> blue fruit into <color=#F7D64A>(3)</color> glasses of water to create <color=#F7D64A>(3)</color> glasses of blue fruit juice.";
    public string PastaCreationDescription = "<b>Boil some pasta</b> - Use <color=#F7D64A>(3)</color> Dried Noodles from your hand to boil it into <color=#F7D64A>(3)</color> Bowls of Pasta.";
    public string GlassWaterCreationDescription = "<b>Glass of Water</b> - <color=#F7D64A>(5)</color> Glasses of Boiled Water collected and heated in your cauldron. Standard stuff.";
    public int DisSatisfiedCustomerCounters = 3;
    public float CustomerSatisfactionXpBonus = 50f;
    public bool DashIndicator = false;
    public bool AnyMealWillDoIndicator = false;

    public int FruitGain = 2; 
    public int WaterGain = 1; 
    public int BlueFruitJuiceGain = 3; 
    public int AcidFlyGain = 1; 
    public int SlicedBlueFruitGain = 2; 
    public int NoodleGain = 3; 
    public int DeAcidFlyGain = 5; 
    public int WaterGlassGain = 5;
    public int PastaGain = 3;
    public float CraftingTimeDelay = 5f;
    public float GatheringTimeDelay = 3f;
    public float CookingTimeDelay = 15f;

    public float xpGain = 20;
    public int maxTip = 3;

    public float CustomerMovementSpeed = 20f; 
    public float LifeTimer = 80f;
    public int DrakeChance = 50;
    public int GoblinChance = 0;
    public int AntiniumChance = 50;
    public bool StayAwhile = false;

    public List<int[]> TableFood = new List<int[]>();
    public List<bool> activeSkills = new List<bool>();
    public List<int> MarketPrices = new List<int>();
    public List<int> FoodPrices = new List<int>();

    public bool canAnyMeal = false;
    public bool canDash = false;

    //stats
    public float steps = 0;
    public float lifts = 0;
    public float chopped = 0;
    public float gathered = 0;
    public float cooked = 0;
    public float drakes = 0;
    public float antinium = 0;
    public float goblins = 0;
    public float customerWait = 0;
    public float customerSteps = 0;
    public float purchases = 0;
    public float ExpensiveFood = 0;
    public float MarketTime = 0;
    public float numofDisatisfiedCustomers = 0;
    public float numofSatisfiedCustomers = 0;
    public float leftovers = 0;
    public float cauldronFilled = 0;
    public float cauldronBoiled = 0;
    public float mealsServed = 0;
}
