using _Project.Scripts.ScriptableObjects.Int;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Hud
{
    public class ShowPensionRating : MonoBehaviour
    {
        [SerializeField] private Image image;
        [Space]
        
        [SerializeField] private IntSo pensionRatingIntObjectSo;

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
