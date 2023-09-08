using UnityEngine;

namespace _Project.Scripts.Gameplay.Animation
{
    public class AnimBoolChange : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string boolName;

        public void SetBool(bool val)
        {
            animator.SetBool(boolName, val);
        }
    }
}
