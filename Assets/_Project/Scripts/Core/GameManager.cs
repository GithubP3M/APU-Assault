using UnityEngine;
using UnityEngine.Events;

namespace APUAssault.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public UnityEvent<int> onScoreChanged;

        public int Score { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void AddScore(int points)
        {
            Score += points;
            onScoreChanged?.Invoke(Score);
        }
    }
}
