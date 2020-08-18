using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<string> PlayerSkills = new List<string>();
    public List<string> Purchases = new List<string>();
    public float PreviousXp = 0;
    public float xp = 0;
    public int Level = 0;
    public int money = 0;

    public float SpawnTimerIncreaseAmount = .92f;
    public int DayCount = 0;
    public int DayTimeLimit = 80;

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
}
