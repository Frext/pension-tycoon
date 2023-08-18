using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ShowPensionRating : MonoBehaviour
    {
        [SerializeField] private Image image;
        [Space]
        
        [SerializeField] private IntObject pensionRatingIntObjectSo;

        void Start()
        {
            UpdateImage();
        }

        public void UpdateImage()
        {
            image.fillAmount = pensionRatingIntObjectSo.GetRatio();
        }
    }
}
