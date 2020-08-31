using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public int Base;
    public int Level;
    public float Modifier
    {
        get { return ModSkill(); }
    }
    public string Description;
    public Vector2 PreferedToolTipSize;
    public Transform PrerequisiteSkill;

    private float ModSkill()
    {
        GameManager player = GameObject.Find("Player").GetComponent<GameManager>();
        if (this.gameObject.name.Equals("Quick Movement") || this.gameObject.name.Equals("Enhanced Movement") || this.gameObject.name.Equals("Diner Dash"))
        {
            return player.steps / 20000;
        }
        else if (this.gameObject.name.Equals("Lesser Strength") || this.gameObject.name.Equals("Enhanced Strength"))
        {
            return player.lifts / 5000;
        }
        else if (this.gameObject.name.Equals("Basic Preparation") || this.gameObject.name.Equals("Advanced Prepation"))
        {
            return player.chopped / 10;
        }
        else if (this.gameObject.name.Equals("Basic Gathering") || this.gameObject.name.Equals("Advanced Gathering") || this.gameObject.name.Equals("Ready To Cook"))
        {
            return player.gathered / 10;
        }
        else if (this.gameObject.name.Equals("Basic Cooking") || this.gameObject.name.Equals("Advanced Cooking"))
        {
            return player.cooked / 5;
        }
        else if (this.gameObject.name.Equals("Customer Preference - Antinium"))
        {
            return player.antinium / 5;
        }
        else if (this.gameObject.name.Equals("Customer Preference - Drake"))
        {
            return player.drakes / 5;
        }
        else if (this.gameObject.name.Equals("Customer Preference - Goblin"))
        {
            return player.goblins / 5;
        }
        else if (this.gameObject.name.Equals("One More Portion"))
        {
            return (player.gathered + player.cooked + player.chopped) / 30;
        }
        else if (this.gameObject.name.Equals("Inn - Calming Presence"))
        {
            return player.customerWait / 300;
        }
        else if (this.gameObject.name.Equals("Magnified Training"))
        {
            return GameObject.Find("Player").GetComponent<PlayerBehavior>().xp / 100;
        }
        else if (this.gameObject.name.Equals("Inn - Lethargic Steps"))
        {
            return player.customerSteps / 10;
        }
        else if (this.gameObject.name.Equals("Discount Runner"))
        {
            return player.purchases / 3;
        }
        else if (this.gameObject.name.Equals("Fancy Food"))
        {
            return player.ExpensiveFood / 1.5f;
        }
        else if (this.gameObject.name.Equals("Proficiency - Haggling"))
        {
            return player.MarketTime / 200;
        }
        else if (this.gameObject.name.Equals("Local Landmark - Liscor") || this.gameObject.name.Equals("Stay Awhile"))
        {
            return player.numofDisatisfiedCustomers;
        }
        else if (this.gameObject.name.Equals("Inn - Generous Tippers"))
        {
            return player.numofSatisfiedCustomers / 3;
        }
        else if (this.gameObject.name.Equals("Field of Preservation"))
        {
            return player.leftovers / 1.5f;
        }
        else if (this.gameObject.name.Equals("Fill Container - Water"))
        {
            return player.cauldronFilled;
        }
        else if (this.gameObject.name.Equals("Quick Boiling"))
        {
            return player.cauldronBoiled;
        }
        else if (this.gameObject.name.Equals("Any Meal Will Do") || this.gameObject.name.Equals("Inn, My Hand"))
        {
            return player.mealsServed / 3f;
        }
        Debug.LogError("Could not find " + this.gameObject.name);
        return 0;
    }
}
