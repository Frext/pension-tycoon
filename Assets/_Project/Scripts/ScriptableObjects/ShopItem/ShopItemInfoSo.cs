using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.ShopItem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(ShopItemInfoSo))]
    public class ShopItemInfoSo : ScriptableObject
    {
        public string itemName;
        public string itemDescription;

        public void SetTo(ShopItemInfoSo newShopItem)
        {
            itemName = newShopItem.itemName;
            itemDescription = newShopItem.itemDescription;
        }
    }
}
