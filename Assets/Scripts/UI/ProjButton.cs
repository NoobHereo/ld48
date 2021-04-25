using UnityEngine;
using UnityEngine.UI;
using game.Objects;

namespace game.UI
{
    public class ProjButton : MonoBehaviour
    {
        public ProjectileCfg ProjCfg;
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(onClick);
        }

        private void onClick()
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.IncreaseStat(PlayerStats.DPS, player.CurrentProj.DPS);
            player.IncreaseStat(PlayerStats.DMG, -player.CurrentProj.DMG);
            player.CurrentProj = ProjCfg;
            player.IncreaseStat(PlayerStats.DPS, -ProjCfg.DPS);
            player.IncreaseStat(PlayerStats.DMG, ProjCfg.DMG);
            player.ProjTexture = ProjCfg.Texture;
        }
    }
}