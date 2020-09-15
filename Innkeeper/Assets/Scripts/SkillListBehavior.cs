using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListBehavior : MonoBehaviour
{
    private Transform Player;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleActive()
    {
        if(Player == null)
        {
            Player = GameObject.Find("Player").transform;
        }
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    private void OnEnable()
    {
        int skillNum = 0;
        Transform SkillArea = null;
        foreach (string skill in Player.GetComponent<PlayerBehavior>().PlayerSkills)
        {
            this.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(skillNum).GetChild(0).GetComponent<Text>().text = skill;
            this.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(skillNum).GetChild(1).GetComponent<Text>().text =
                Player.GetComponent<GameManager>().LevelChoices.GetComponent<LevelManager>().SkillDictionary[skill].GetComponent<SkillBehavior>().Description;
            if (this.transform.GetChild(1).GetChild(0).GetChild(0).childCount <= skillNum + 1)
            {
                SkillArea = Instantiate(this.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(skillNum));
                SkillArea.SetParent(this.transform.GetChild(1).GetChild(0).GetChild(0), false);
            }
            skillNum++;
        }
        Destroy(SkillArea.gameObject);
    }
}
