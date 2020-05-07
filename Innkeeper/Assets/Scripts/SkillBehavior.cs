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

    private float ModSkill()
    {
        GameManager player = GameObject.Find("Player").GetComponent<GameManager>();
        if (this.gameObject.name.Equals("Lesser Speed") || this.gameObject.name.Equals("Enhanced Speed"))
        {
            return player.steps / 20000;
        }
        else if (this.gameObject.name.Equals("Lesser Strength") || this.gameObject.name.Equals("Enhanced Strength"))
        {
            return player.lifts / 5000;
        }
        else if (this.gameObject.name.Equals("Basic Chopping") || this.gameObject.name.Equals("Advanced Chopping"))
        {
            return player.chopped / 10;
        }
        else if (this.gameObject.name.Equals("Basic Gathering") || this.gameObject.name.Equals("Advanced Gathering"))
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
        else if (this.gameObject.name.Equals("Extra Portion"))
        {
            return (player.gathered + player.cooked + player.chopped) / 30;
        }
        else if (this.gameObject.name.Equals("Inn - Calming Aura"))
        {
            return player.customerWait / 300;
        }
        else if (this.gameObject.name.Equals("Magnified Training"))
        {
            return GameObject.Find("Player").GetComponent<PlayerBehavior>().xp / 100;
        }
        Debug.LogError("Could not find" + this.gameObject.name);
        return 0;
    }
}
