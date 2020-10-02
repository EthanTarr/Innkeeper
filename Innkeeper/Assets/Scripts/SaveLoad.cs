using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public static class SaveLoad
{
    public static void Save()
    {
        Save save = new Save();
        PlayerBehavior Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        save.PlayerSkills = Player.PlayerSkills;
        save.MovementSpeed = Player.MovementSpeed;
        save.Purchases = Player.Purchases;
        save.PreviousXp = Player.PreviousXp;
        save.xp = Player.xp;
        save.Level = Player.Level;
        save.money = Player.money;
        save.strength = Player.strength;

        GameManager Game = GameObject.Find("Player").GetComponent<GameManager>();
        save.SpawnTimerIncreaseAmount = Game.SpawnTimerIncreaseAmount;
        save.DayCount = Game.DayCount;
        save.DayTimeLimit = Game.DayTimeLimit;
        save.DayStartDelay = Game.DayStartDelay;
        save.xpBarPercentage = Game.EndOfDayScreen.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().originalPercentage;
        save.UnlockableFoods = Game.UnlockableFoods;
        save.BlueFruitGatherDescription = Game.DoorPopup.GetChild(3).GetComponent<Info>().Description;
        save.WaterGatherDescription = Game.DoorPopup.GetChild(4).GetComponent<Info>().Description;
        save.AcidFlyGatherDescription = Game.DoorPopup.GetChild(6).GetComponent<Info>().Description;
        save.NoodleGatherDescription = Game.DoorPopup.GetChild(5).GetComponent<Info>().Description;
        save.DeAcidFlyCreationDescription = Game.CraftingPopup.GetChild(3).GetComponent<Info>().Description;
        save.SliceBlueFruitCreationDescription = Game.CraftingPopup.GetChild(4).GetComponent<Info>().Description;
        save.BlueFruitJuiceCreationDescription = Game.CraftingPopup.GetChild(5).GetComponent<Info>().Description;
        save.PastaCreationDescription = Game.CauldronPopups[0].GetChild(3).GetComponent<Info>().Description;
        save.GlassWaterCreationDescription = Game.CauldronPopups[0].GetChild(4).GetComponent<Info>().Description;
        save.DisSatisfiedCustomerCounters = Game.DisSatisfiedCustomerCounters.Count;
        save.CustomerSatisfactionXpBonus = Game.CustomerSatisfactionXpBonus;
        save.DashIndicator = Game.DashIndicator.gameObject.activeSelf;
        save.AnyMealWillDoIndicator = Game.AnyMealWillDoIndicator.gameObject.activeSelf;

        ResourceManager Resource = GameObject.Find("Player").GetComponent<ResourceManager>();
        save.FruitGain = Resource.FruitGain;
        save.WaterGain = Resource.WaterGain;
        save.BlueFruitJuiceGain = Resource.BlueFruitJuiceGain;
        save.AcidFlyGain = Resource.AcidFlyGain;
        save.SlicedBlueFruitGain = Resource.SlicedBlueFruitGain;
        save.NoodleGain = Resource.NoodleGain;
        save.DeAcidFlyGain = Resource.DeAcidFlyGain;
        save.WaterGlassGain = Resource.WaterGlassGain;
        save.PastaGain = Resource.PastaGain;
        save.CraftingTimeDelay = Resource.CraftingTimeDelay;
        save.GatheringTimeDelay = Resource.GatheringTimeDelay;
        save.CookingTimeDelay = Resource.CookingTimeDelay;

        CustomerBehavior Customer = Game.Customer.GetComponent<CustomerBehavior>();
        save.CustomerMovementSpeed = Customer.MovementSpeed;
        save.LifeTimer = Customer.LifeTimer;
        save.DrakeChance = Customer.DrakeChance;
        save.GoblinChance = Customer.GoblinChance;
        save.AntiniumChance = Customer.AntiniumChance;
        save.StayAwhile = Customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).gameObject.activeSelf;

        CustomerRequestBehavior Request = Game.Customer.GetComponent<PopUpObjectBehavior>().Popup.GetChild(3).GetComponent<CustomerRequestBehavior>();
        save.xpGain = Request.xpGain;
        save.maxTip = Request.maxTip;

        save.TableFood = Game.tableToSave();
        save.activeSkills = Game.encodeActiveSkills();
        Transform content = Game.MarketScreen.transform.GetChild(1).GetChild(0).GetChild(0);
        for (int i = 0; i < content.childCount; i++)
        {
            save.MarketPrices.Add(content.GetChild(i).GetChild(1).GetComponent<PurchaseInfo>().Cost);
        }
        foreach(Transform food in Resource.Foods)
        {
            save.FoodPrices.Add(food.GetComponent<ItemBehavior>().ItemValue);
        }

        save.canAnyMeal = Game.canAnyMeal;
        save.canDash = Player.canDash;

        //stats
        save.steps = Game.steps;
        save.lifts = Game.lifts;
        save.chopped = Game.chopped;
        save.gathered = Game.gathered;
        save.cooked = Game.cooked;
        save.drakes = Game.drakes;
        save.antinium = Game.antinium;
        save.goblins = Game.goblins;
        save.customerWait = Game.customerWait;
        save.customerSteps = Game.customerSteps;
        save.purchases = Game.purchases;
        save.ExpensiveFood = Game.ExpensiveFood;
        save.MarketTime = Game.MarketTime;
        save.numofDisatisfiedCustomers = Game.numofDisatisfiedCustomers;
        save.numofSatisfiedCustomers = Game.numofSatisfiedCustomers;
        save.leftovers = Game.leftovers;
        save.cauldronFilled = Game.cauldronFilled;
        save.cauldronBoiled = Game.cauldronBoiled;
        save.mealsServed = Game.mealsServed;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.ent");
        bf.Serialize(file, save);
        file.Close();
    }

    public static void Load()
    {
        Load("/saveGame.ent");
    }

    public static void Load(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            GameObject oldPlayer = GameObject.Find("Player");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();


            PlayerBehavior Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
            GameManager Game = GameObject.Find("Player").GetComponent<GameManager>();
            LevelManager level = Game.LevelChoices.GetComponent<LevelManager>();
            ResourceManager Resource = GameObject.Find("Player").GetComponent<ResourceManager>();
            CustomerRequestBehavior Request = Game.Customer.GetComponent<PopUpObjectBehavior>().Popup.GetChild(3).GetComponent<CustomerRequestBehavior>();
            CustomerBehavior Customer = Game.Customer.GetComponent<CustomerBehavior>();

            foreach (string skill in save.PlayerSkills)
            {
                level.Calls[skill]();
            }
            Transform MarketPurchases = Game.MarketScreen.GetChild(1).GetChild(0).GetChild(0);
            foreach (string purch in save.Purchases)
            {
                for (int i = 0; i < MarketPurchases.childCount; i++)
                {
                    if (MarketPurchases.GetChild(i).name.Equals(purch))
                    {
                        MarketPurchases.GetChild(i).GetChild(3).GetComponent<Button>().onClick.Invoke();
                        break;
                    }
                }
            }
            for (int i = 0; i < MarketPurchases.childCount; i++)
            {
                MarketPurchases.GetChild(i).GetChild(1).GetComponent<PurchaseInfo>().Cost = save.MarketPrices[i];
                MarketPurchases.GetChild(i).GetChild(1).GetComponent<PurchaseInfo>().updateCost();
            }
            Player.MovementSpeed = save.MovementSpeed;
            Player.PreviousXp = save.PreviousXp;
            Player.xp = save.xp;
            Player.Level = save.Level;
            Player.money = save.money;
            Player.strength = save.strength;

            Game.SpawnTimerIncreaseAmount = save.SpawnTimerIncreaseAmount;
            Game.DayCount = save.DayCount - 1;
            Game.DayTimeLimit = save.DayTimeLimit;
            Game.DayStartDelay = save.DayStartDelay;
            Game.EndOfDayScreen.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<XpBarBehavior>().originalPercentage = save.xpBarPercentage;
            Game.UnlockableFoods = save.UnlockableFoods;
            Game.CustomerSatisfactionXpBonus = save.CustomerSatisfactionXpBonus;
            Game.DashIndicator.gameObject.SetActive(save.DashIndicator);
            Game.AnyMealWillDoIndicator.gameObject.SetActive(save.AnyMealWillDoIndicator);

            Game.DoorPopup.GetChild(3).GetComponent<Info>().Description = save.BlueFruitGatherDescription;
            Game.DoorPopup.GetChild(4).GetComponent<Info>().Description = save.WaterGatherDescription;
            Game.DoorPopup.GetChild(6).GetComponent<Info>().Description = save.AcidFlyGatherDescription;
            Game.DoorPopup.GetChild(5).GetComponent<Info>().Description = save.NoodleGatherDescription;
            Game.CraftingPopup.GetChild(3).GetComponent<Info>().Description = save.DeAcidFlyCreationDescription;
            Game.CraftingPopup.GetChild(4).GetComponent<Info>().Description = save.SliceBlueFruitCreationDescription;
            Game.CraftingPopup.GetChild(5).GetComponent<Info>().Description = save.BlueFruitJuiceCreationDescription;
            foreach (Transform cauldronPopup in Game.CauldronPopups)
            {
                cauldronPopup.GetChild(3).GetComponent<Info>().Description = save.PastaCreationDescription;
                cauldronPopup.GetChild(4).GetComponent<Info>().Description = save.GlassWaterCreationDescription;
            }
            for(int i = save.DisSatisfiedCustomerCounters; i > 3; i--)
            {
                Game.createDisSatisfiedCustomerCounters();
            }
            for (int i = 0; i < Resource.Foods.Count; i++)
            {
                Resource.Foods[i].GetComponent<ItemBehavior>().ItemValue = save.FoodPrices[i];
            }

            Resource.FruitGain = save.FruitGain;
            Resource.WaterGain = save.WaterGain;
            Resource.BlueFruitJuiceGain = save.BlueFruitJuiceGain;
            Resource.AcidFlyGain = save.AcidFlyGain;
            Resource.SlicedBlueFruitGain = save.SlicedBlueFruitGain;
            Resource.NoodleGain = save.NoodleGain;
            Resource.DeAcidFlyGain = save.DeAcidFlyGain;
            Resource.WaterGlassGain = save.WaterGlassGain;
            Resource.PastaGain = save.PastaGain;
            Resource.CraftingTimeDelay = save.CraftingTimeDelay;
            Resource.GatheringTimeDelay = save.GatheringTimeDelay;
            Resource.CookingTimeDelay = save.CookingTimeDelay;

            Customer.MovementSpeed = save.CustomerMovementSpeed;
            Customer.LifeTimer = save.LifeTimer;
            Customer.DrakeChance = save.DrakeChance;
            Customer.GoblinChance = save.GoblinChance;
            Customer.AntiniumChance = save.AntiniumChance;
            Customer.GetComponent<PopUpObjectBehavior>().Popup.transform.GetChild(4).gameObject.SetActive(save.StayAwhile);

            Request.xpGain = save.xpGain;
            Request.maxTip = save.maxTip;

            Game.savetoTable(save.TableFood);
            Game.decodeActiveSkills(save.activeSkills);

            Game.canAnyMeal = save.canAnyMeal;
            Player.canDash = save.canDash;

            //stats
            Game.steps = save.steps;
            Game.lifts = save.lifts;
            Game.chopped = save.chopped;
            Game.gathered = save.gathered;
            Game.cooked = save.cooked;
            Game.drakes = save.drakes;
            Game.antinium = save.antinium;
            Game.goblins = save.goblins;
            Game.customerWait = save.customerWait;
            Game.customerSteps = save.customerSteps;
            Game.purchases = save.purchases;
            Game.ExpensiveFood = save.ExpensiveFood;
            Game.MarketTime = save.MarketTime;
            Game.numofDisatisfiedCustomers = save.numofDisatisfiedCustomers;
            Game.numofSatisfiedCustomers = save.numofSatisfiedCustomers;
            Game.leftovers = save.leftovers;
            Game.cauldronFilled = save.cauldronFilled;
            Game.cauldronBoiled = save.cauldronBoiled;
            Game.mealsServed = save.mealsServed;
        }
    }

    private static Save blankStats()
    {
        Save save = new Save();
        PlayerBehavior Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        save.PlayerSkills = new List<string>();
        save.Purchases = new List<string>();
        save.MovementSpeed = 1.5f;
        save.PreviousXp = 0;
        save.xp = 0;
        save.Level = 0;
        save.money = 0;

        GameManager Game = GameObject.Find("Player").GetComponent<GameManager>();
        save.SpawnTimerIncreaseAmount = .99f;
        save.DayCount = 1;
        save.DayTimeLimit = 100;
        save.DayStartDelay = 15;
        save.xpBarPercentage = 0;
        save.strength = 0;
        save.UnlockableFoods = Game.populateUnlockableFoods();
        save.BlueFruitGatherDescription = "<b>Gather Blue Fruits</b> - Gather <color=#F7D64A>(2)</color> Blue Fruits from a nearby orchird of trees. They seem edible.";
        save.WaterGatherDescription = "<b>Gather Water</b> - Gather <color=#F7D64A>(1)</color> Bucket of Water from the nearby stream.";
        save.AcidFlyGatherDescription = "<b>Gather Acid Flys</b> - Gather <color=#F7D64A>(1)</color> Jar of captured Acid Flys.";
        save.NoodleGatherDescription = "<b>Gather Noodle Ingredients</b> - Gather the ingredients for <color=#F7D64A>(3)</color> Noodles from the nearby area.";
        save.DeAcidFlyCreationDescription = "<b>Seperate Acid from Flies</b> - Violently shake <color=#F7D64A>(1)</color> Jar of Acid Flies to kill the fragile creatures. Then seperate the fly corpses from the acid and place them into <color=#F7D64A>(5)</color> bowls.";
        save.SliceBlueFruitCreationDescription = "<b>Slice Blue Fruit</b> - Slice <color=#F7D64A>(1)</color> Blue Fruit into <color=#F7D64A>(2)</color> Meals of Sliced Blue Fruit";
        save.BlueFruitJuiceCreationDescription = "<b>Blue Fruit Juice</b> - Juice <color=#F7D64A>(1)</color> blue fruit into <color=#F7D64A>(3)</color> glasses of water to create <color=#F7D64A>(3)</color> glasses of blue fruit juice.";
        save.PastaCreationDescription = "<b>Boil some pasta</b> - Use <color=#F7D64A>(3)</color> Dried Noodles from your hand to boil it into <color=#F7D64A>(3)</color> Bowls of Pasta.";
        save.GlassWaterCreationDescription = "<b>Glass of Water</b> - <color=#F7D64A>(5)</color> Glasses of Boiled Water collected and heated in your cauldron. Standard stuff.";
        save.DisSatisfiedCustomerCounters = 3;
        save.CustomerSatisfactionXpBonus = 50f;
        save.DashIndicator = false;
        save.AnyMealWillDoIndicator = false;

        save.FruitGain = 2;
        save.WaterGain = 1;
        save.BlueFruitJuiceGain = 3;
        save.AcidFlyGain = 1;
        save.SlicedBlueFruitGain = 2;
        save.NoodleGain = 3;
        save.DeAcidFlyGain = 5;
        save.WaterGlassGain = 5;
        save.PastaGain = 3;
        save.CraftingTimeDelay = 5f;
        save.GatheringTimeDelay = 3f;
        save.CookingTimeDelay = 15f;

        save.CustomerMovementSpeed = 20f;
        save.LifeTimer = 100f;
        save.DrakeChance = 50;
        save.GoblinChance = 0;
        save.AntiniumChance = 50;
        save.StayAwhile = false;

        save.xpGain = 20;
        save.maxTip = 3;

        save.TableFood = Game.tableToSave();
        save.activeSkills = Game.encodeActiveSkills();
        save.MarketPrices = new List<int>();
        save.FoodPrices = new List<int>();
        Transform content = Game.MarketScreen.transform.GetChild(1).GetChild(0).GetChild(0);
        ResourceManager Resource = GameObject.Find("Player").GetComponent<ResourceManager>();
        for (int i = 0; i < content.childCount; i++)
        {
            save.MarketPrices.Add(content.GetChild(i).GetChild(1).GetComponent<PurchaseInfo>().Cost);
        }
        foreach (Transform food in Resource.Foods)
        {
            save.FoodPrices.Add(food.GetComponent<ItemBehavior>().ItemValue);
        }

        save.canAnyMeal = false;
        save.canDash = false;

        //stats
        save.steps = 0;
        save.lifts = 0;
        save.chopped = 0;
        save.gathered = 0;
        save.cooked = 0;
        save.drakes = 0;
        save.antinium = 0;
        save.goblins = 0;
        save.customerWait = 0;
        save.customerSteps = 0;
        save.purchases = 0;
        save.ExpensiveFood = 0;
        save.MarketTime = 0;
        save.numofDisatisfiedCustomers = 0;
        save.numofSatisfiedCustomers = 0;
        save.leftovers = 0;
        save.cauldronFilled = 0;
        save.cauldronBoiled = 0;
        save.mealsServed = 0;

        return save;
    }

        public static void SaveBlank()
    {
        Save save = blankStats();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/blankGame.ent");
        bf.Serialize(file, save);
        file.Close();
    }

    public static void reset()
    {
        Save save = blankStats();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.ent");
        bf.Serialize(file, save);
        file.Close();
    }
}
