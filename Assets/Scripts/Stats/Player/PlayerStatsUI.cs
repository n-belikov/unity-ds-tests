using UnityEngine;
using UnityEngine.UI;

namespace Stats.Player
{
    public class PlayerStatsUI : MonoBehaviour
    {
        public Image Image;

        public void SetFillImage(int current, int max)
        {
            Image.fillAmount = (float)current / (float)max;
        }
    }
}