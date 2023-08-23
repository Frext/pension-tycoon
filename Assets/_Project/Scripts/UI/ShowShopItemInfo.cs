using _Project.Scripts.ScriptableObjects.ShopItem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ShowShopItemInfo : MonoBehaviour
    {
        [SerializeField] private GameObject parentObject;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [Space] 
        [SerializeField] private ShopItem selectedShopItemSo;

        public void ShowItem()
        {
            parentObject.SetActive(true);
            
            itemNameText.text = selectedShopItemSo.itemName;
            itemDescriptionText.text = selectedShopItemSo.itemDescription;
        }
    }
}
