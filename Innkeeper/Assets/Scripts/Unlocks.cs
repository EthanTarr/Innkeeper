using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unlocks : MonoBehaviour
{
    private GameManager gm;

    public Sprite PastaSprite;
    public Sprite AcidFlySprite;

    // Start is called before the first frame update
    void Start()
    {
        gm = this.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void unlockPasta()
    {
        gm.UnlockedFoodScreen.GetChild(1).GetChild(0).GetComponent<Text>().text = "Pasta!";
        gm.UnlockedFoodScreen.GetChild(1).GetChild(1).GetComponent<Image>().sprite = PastaSprite;
        gm.UnlockedFoodScreen.gameObject.SetActive(true);
        Debug.Log("test " + gm.UnlockedFoodScreen.gameObject.activeSelf);
        gm.PurchaseBoxPupup.GetComponent<PurchasePopupBehavior>().activatePasta();
        foreach(Transform cauldron in gm.CauldronPopups)
        {
            cauldron.GetComponent<CauldronPopupBehavor>().activatePasta();
        }
        gm.DoorPopup.GetComponent<DoorPopupBehavior>().activatePasta();
        for (int i = 0; i < gm.MarketScreen.GetChild(1).GetChild(0).GetChild(0).childCount; i++)
        {
            if (gm.MarketScreen.GetChild(1).GetChild(0).GetChild(0).GetChild(i).name.Equals("Flour"))
            {
                gm.MarketScreen.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void unlockAcidFlys()
    {
        gm.UnlockedFoodScreen.GetChild(1).GetChild(0).GetComponent<Text>().text = "Acid Flys!";
        gm.UnlockedFoodScreen.GetChild(1).GetChild(1).GetComponent<Image>().sprite = AcidFlySprite;
        gm.UnlockedFoodScreen.gameObject.SetActive(true);
        gm.CraftingPopup.GetComponent<CraftingPopupBehavior>().activateFlys();
        gm.PurchaseBoxPupup.GetComponent<PurchasePopupBehavior>().activateFlys();
        gm.DoorPopup.GetComponent<DoorPopupBehavior>().activateFlys();
        for (int i = 0; i < gm.MarketScreen.GetChild(1).GetChild(0).GetChild(0).childCount; i++)
        {
            if (gm.MarketScreen.GetChild(1).GetChild(0).GetChild(0).GetChild(i).name.Equals("Jar"))
            {
                gm.MarketScreen.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }
}
