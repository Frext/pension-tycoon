using _Project.Scripts.Gameplay.NPC;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ShowDayProgress : MonoBehaviour
    {
        [SerializeField] private Image image;

        [SerializeField] private NPCManager npcManagerScript;

        void Awake()
        {
            ResetProgressBar();
        }

        public void UpdateProgressBar()
        {
            int totalEnemyWaveSize = npcManagerScript.GetTotalEnemyWaveSize();
            
            image.fillAmount = (float)(totalEnemyWaveSize - npcManagerScript.GetCurrentEnemyWaveSize() ) / totalEnemyWaveSize;
        }

        public void ResetProgressBar()
        {
            image.fillAmount = 0f;
        }
    }
}
