using UnityEngine;

using GameJam.HealthSystem;
using GameJam.UI;

namespace GameJam
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private EndScreen endScreen;
        [SerializeField] private StartScreen startScreen;
        [SerializeField] private int nextLevelIndex;

        [SerializeField] private Health[] enemies;

        [SerializeField] private int calmed;
        [SerializeField] private int dead;

        private void OnEnable()
        {
            foreach (var enemy in enemies)
            {
                enemy.OnCalm += CountCalmed;
                enemy.OnDeath += HandleDeath;
            }

            startScreen.Show();
            endScreen.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var enemy in enemies)
            {
                enemy.OnCalm -= CountCalmed;
                enemy.OnDeath -= HandleDeath;
            }
        }

        private void CountCalmed()
        {
            calmed = 0;

            foreach (var enemy in enemies)
            {
                if (enemy != null && enemy.Calmed)
                {
                    calmed++;
                }
            }

            if (dead + calmed == enemies.Length)
            {
                endScreen.Show(
                    playerDead: false,
                    enemies: enemies.Length,
                    calmedEnemies: calmed,
                    deadEnemies: dead,
                    nextLevelIndex: nextLevelIndex);
            }
        }

        private void HandleDeath()
        {
            dead++;
            CountCalmed();

            if (dead + calmed == enemies.Length)
            {
                endScreen.Show(
                    playerDead: false,
                    enemies: enemies.Length,
                    calmedEnemies: calmed,
                    deadEnemies: dead,
                    nextLevelIndex: nextLevelIndex);
            }
        }
    }
}
