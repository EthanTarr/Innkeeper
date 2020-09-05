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
        gm.PurchaseBoxPupup.GetComponent<PurchasePopupBehavior>().activatePasta();
        foreach(Transform cauldron in gm.CauldronPopups)
        {
            cauldron.GetComponent<CauldronPopupBehavor>().activatePasta();
        }
        gm.DoorPopup.GetComponent<DoorPopupBehavior>().activatePasta();
    }

    public void unlockAcidFlys()
    {
        gm.UnlockedFoodScreen.GetChild(1).GetChild(0).GetComponent<Text>().text = "Acid Flys!";
        gm.UnlockedFoodScreen.GetChild(1).GetChild(1).GetComponent<Image>().sprite = AcidFlySprite;
        gm.UnlockedFoodScreen.gameObject.SetActive(true);
        gm.CraftingPopup.GetComponent<CraftingPopupBehavior>().activateFlys();
        gm.PurchaseBoxPupup.GetComponent<PurchasePopupBehavior>().activateFlys();
        gm.DoorPopup.GetComponent<DoorPopupBehavior>().activateFlys();
    }
}
