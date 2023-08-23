using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.ShopItem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(ShopItem))]
    public class ShopItem : ScriptableObject
    {
        public string itemName;
        public string itemDescription;

        public void SetTo(ShopItem newShopItem)
        {
            itemName = newShopItem.itemName;
            itemDescription = newShopItem.itemDescription;
        }
    }
}
