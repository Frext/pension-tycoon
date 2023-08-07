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
        
        [SerializeField] private SoEvent OnChangePensionRating;

        void Awake()
        {
            OnChangePensionRating.RegisterToEvent(UpdateImage);
        }

        void Start()
        {
            UpdateImage();
        }

        void OnDestroy()
        {
            OnChangePensionRating.DeregisterFromEvent(UpdateImage);
        }

        void UpdateImage()
        {
            image.fillAmount = (float)pensionRatingIntObjectSo.Value / pensionRatingIntObjectSo.GetMaxValue();
        }
    }
}
