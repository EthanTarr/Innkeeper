using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanMakeButton : MonoBehaviour
{
    private Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.gameObject.activeSelf)
        {
            bool itemCraftable = false;
            Dictionary<string, int> Ingredients = new Dictionary<string, int>();

            if (this.GetComponent<Image>().sprite.name.Equals("blue_fruit_juice"))
            {
                Ingredients.Add("WaterGlass", 3);
                Ingredients.Add("Blue Fruit", 1);
            }
            else if (this.GetComponent<Image>().sprite.name.Equals("blue_fruit_slice"))
            {
                Ingredients.Add("Blue Fruit", 1);
            }
            else if (this.GetComponent<Image>().sprite.name.Equals("Fly in a Bowl"))
            {
                Ingredients.Add("Acid Fly", 1);
            }

            Transform CraftingSurface = this.transform.parent.GetComponent<PopupBehaviour>().PopupObject;
            if (!itemCraftable)
            {
                itemCraftable = Check(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, Ingredients, CraftingSurface);
            }
            if (!itemCraftable)
            {
                itemCraftable = Check(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, Ingredients, CraftingSurface);
            }
            if (!itemCraftable)
            {
                itemCraftable = Check(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, Ingredients, CraftingSurface);
            }
            this.GetComponent<Button>().interactable = itemCraftable;


            if (this.name.Equals("DeAcid Fly Gather Action") && Player.GetComponent<PlayerBehavior>().MovementSpeed <
                ((.2f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 5))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else if (this.name.Equals("Slice Blue Fruit Gather Action") && Player.GetComponent<PlayerBehavior>().MovementSpeed <
                ((.08f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 2))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else if (this.name.Equals("Blue Fruit Juice Gather Action") && Player.GetComponent<PlayerBehavior>().MovementSpeed <
                ((.4f - GameObject.Find("Player").GetComponent<PlayerBehavior>().strength) * 1))
            {
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.GetComponent<Button>().interactable = true;
            }
        }
    }

    private bool Check(Transform Ingredient, Dictionary<string, int> DesiredIngredients, Transform CraftingSurface)
    {
        if (Ingredient != null && DesiredIngredients.ContainsKey(Ingredient.name) && DesiredIngredients[Ingredient.name] <= Ingredient.GetComponent<ItemBehavior>().ItemCount)
        {
            Dictionary<Transform, int> GatheredObjects = new Dictionary<Transform, int>();
            GatheredObjects.Add(Ingredient, DesiredIngredients[Ingredient.name]);
            DesiredIngredients.Remove(Ingredient.name);
            if (DesiredIngredients.Count == 0)
            {
                return true;
            }
            else if (DesiredIngredients.Count == 1)
            {
                return Check2(DesiredIngredients, GatheredObjects, CraftingSurface);
            }
            else
            {
                if (CraftingSurface.GetComponent<StorageBehaviour>().CenterObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.GetComponent<ItemBehavior>().ItemCount))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name]);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name);
                    return Check2(DesiredIngredients, GatheredObjects, CraftingSurface);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().RightObject.GetComponent<ItemBehavior>().ItemCount))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name]);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name);
                    return Check2(DesiredIngredients, GatheredObjects, CraftingSurface);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.GetComponent<ItemBehavior>().ItemCount))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name]);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name);
                    Check2(DesiredIngredients, GatheredObjects, CraftingSurface);
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    private bool Check2(Dictionary<string, int> DesiredIngredients, Dictionary<Transform, int> GatheredObjects, Transform CraftingSurface)
    {
        if (CraftingSurface.GetComponent<StorageBehaviour>().CenterObject != null &&
                    (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.GetComponent<ItemBehavior>().ItemCount))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name]);
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
            (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().RightObject.GetComponent<ItemBehavior>().ItemCount))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name]);
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
            (DesiredIngredients.ContainsKey(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)) &&
                    (DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name] <=
                    CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.GetComponent<ItemBehavior>().ItemCount))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject, DesiredIngredients[CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name]);
            return true;
        }
        return false;
    }
}
