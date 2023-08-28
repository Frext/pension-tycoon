using _Project.Scripts.Gameplay.NPC;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Hud
{
    public class ShowDayProgress : MonoBehaviour
    {
        [SerializeField] private Image image;

        [SerializeField] private NpcManager npcManagerScript;

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
