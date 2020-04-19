using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanMakeButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.gameObject.activeSelf)
        {
            bool itemCraftable = false;
            List<string> Ingredients = new List<string>();

            if (this.GetComponent<Image>().sprite.name.Equals("blue_fruit_juice"))
            {
                Ingredients.Add("Water");
                Ingredients.Add("Blue Fruit");
            }
            else if (this.GetComponent<Image>().sprite.name.Equals("blue_fruit_slice"))
            {
                Ingredients.Add("Blue Fruit");
            }
            else if (this.GetComponent<Image>().sprite.name.Equals("Fly in a Bowl"))
            {
                Ingredients.Add("Acid Fly");
            }
            else if (this.GetComponent<Image>().sprite.name.Equals("pasta_cooked"))
            {
                Ingredients.Add("Water");
                Ingredients.Add("Noodles");
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
        }
    }

    private bool Check(Transform Ingredient, List<string> DesiredIngredients, Transform CraftingSurface)
    {
        if (Ingredient != null && DesiredIngredients.Contains(Ingredient.name))
        {
            List<Transform> GatheredObjects = new List<Transform>();
            GatheredObjects.Add(Ingredient);
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
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name);
                    return Check2(DesiredIngredients, GatheredObjects, CraftingSurface);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name);
                    return Check2(DesiredIngredients, GatheredObjects, CraftingSurface);
                }
                else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)))
                {
                    GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject);
                    DesiredIngredients.Remove(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name);
                    Check2(DesiredIngredients, GatheredObjects, CraftingSurface);
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    private bool Check2(List<string> DesiredIngredients, List<Transform> GatheredObjects, Transform CraftingSurface)
    {
        if (CraftingSurface.GetComponent<StorageBehaviour>().CenterObject != null &&
                    (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject.name)))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().CenterObject);
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().RightObject != null &&
            (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().RightObject.name)))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().RightObject);
            return true;
        }
        else if (CraftingSurface.GetComponent<StorageBehaviour>().LeftObject != null &&
            (DesiredIngredients.Contains(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject.name)))
        {
            GatheredObjects.Add(CraftingSurface.GetComponent<StorageBehaviour>().LeftObject);
            return true;
        }
        return false;
    }
}
