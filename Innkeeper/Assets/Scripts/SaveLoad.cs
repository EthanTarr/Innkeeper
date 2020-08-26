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
        save.Purchases = Player.Purchases;
        save.PreviousXp = Player.PreviousXp;
        save.xp = Player.xp;
        save.Level = Player.Level;
        save.money = Player.money;

        GameManager Game = GameObject.Find("Player").GetComponent<GameManager>();
        save.SpawnTimerIncreaseAmount = Game.SpawnTimerIncreaseAmount;
        save.DayCount = Game.DayCount;
        save.DayTimeLimit = Game.DayTimeLimit;

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
        if (File.Exists(Application.persistentDataPath + "/saveGame.ent"))
        {
            GameObject oldPlayer = GameObject.Find("Player");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame.ent", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();


            PlayerBehavior Player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
            GameManager Game = GameObject.Find("Player").GetComponent<GameManager>();
            LevelManager level = Game.LevelChoices.GetComponent<LevelManager>();
            foreach (string skill in save.PlayerSkills)
            {
                level.Calls[skill]();
            }
            Transform MarketPurchases = Game.EndOfDayScreen.GetComponent<EndOfDayBehavior>().MarketScreen.GetChild(1).GetChild(0).GetChild(0);
            foreach (string purch in save.Purchases)
            {
                for(int i = 0; i < MarketPurchases.childCount; i++)
                {
                    if(MarketPurchases.GetChild(i).name.Equals(purch))
                    {
                        MarketPurchases.GetChild(i).GetChild(3).GetComponent<Button>().onClick.Invoke();
                        break;
                    }
                }
            }
            Player.PreviousXp = save.PreviousXp;
            Player.xp = save.xp;
            Player.Level = save.Level;
            Player.money = save.money;

            
            Game.SpawnTimerIncreaseAmount = save.SpawnTimerIncreaseAmount;
            Game.DayCount = save.DayCount - 1;
            Game.DayTimeLimit = save.DayTimeLimit;

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
}
