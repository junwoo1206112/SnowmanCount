using System.Collections;

using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class BossController : MonoBehaviour
    {
        [Header("Boss Settings")]
        [SerializeField] private int maxHP = 30;
        [SerializeField] private int currentHP;
        [SerializeField] private float hitFlashDuration = 0.1f;

        [Header("Phase Thresholds")]
        [SerializeField] private float phase2Threshold = 0.5f;
        [SerializeField] private float phase3Threshold = 0.25f;

        public int CurrentPhase { get; private set; } = 1;

        private EnemyAllClearDetector clearDetector;
        private Renderer[] bossRenderers;
        private Color[] originalColors;
        private TextMesh hpTextMesh;
        private bool isDead;

        public void Setup(int hp)
        {
            maxHP = hp;
            currentHP = hp;
        }

        private void Start()
        {
            bossRenderers = GetComponentsInChildren<Renderer>();
            if (bossRenderers != null && bossRenderers.Length > 0)
            {
                originalColors = new Color[bossRenderers.Length];
                for (int i = 0; i < bossRenderers.Length; i++)
                {
                    originalColors[i] = bossRenderers[i].material.color;
                }
            }

            CreateHPDisplay();

            clearDetector = FindFirstObjectByType<EnemyAllClearDetector>();
            if (clearDetector != null)
            {
                clearDetector.RegisterEnemy();
            }
        }

        private void CreateHPDisplay()
        {
            GameObject textObj = new GameObject("BossHP_TextMesh");
            textObj.transform.SetParent(transform);
            textObj.transform.localPosition = new Vector3(0, 3.5f, 0);

            hpTextMesh = textObj.AddComponent<TextMesh>();
            hpTextMesh.fontSize = 200;
            hpTextMesh.characterSize = 0.15f;
            hpTextMesh.anchor = TextAnchor.MiddleCenter;
            hpTextMesh.alignment = TextAlignment.Center;
            hpTextMesh.fontStyle = FontStyle.Bold;
            hpTextMesh.color = Color.yellow;

            UpdateHPUI();
        }

        private void UpdateHPUI()
        {
            if (hpTextMesh != null)
            {
                hpTextMesh.text = $"{currentHP}/{maxHP}";
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isDead) return;

            bool isFollower = other.GetComponent<FollowerComponent>() != null;
            bool isPlayer = other.CompareTag("Player");

            if (!isFollower && !isPlayer) return;

            CrowdController crowd = FindFirstObjectByType<CrowdController>();

            if (isPlayer)
            {
                if (currentHP > 0 && GameManager.Instance != null)
                {
                    GameManager.Instance.OnLeaderDied();
                }
            }
            else if (isFollower)
            {
                TakeDamage(1);

                if (crowd != null)
                {
                    crowd.RemoveSpecificFollower(other.gameObject);
                }
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            currentHP -= damage;
            UpdateHPUI();
            StartCoroutine(HitFlashRoutine());

            CheckPhaseTransition();

            if (currentHP <= 0)
            {
                Die();
            }
        }

        private void CheckPhaseTransition()
        {
            float hpPercent = (float)currentHP / maxHP;
            int newPhase = CurrentPhase;

            if (hpPercent <= phase3Threshold)
            {
                newPhase = 3;
            }
            else if (hpPercent <= phase2Threshold)
            {
                newPhase = 2;
            }

            if (newPhase != CurrentPhase)
            {
                CurrentPhase = newPhase;
            }
        }

        private IEnumerator HitFlashRoutine()
        {
            if (bossRenderers != null)
            {
                foreach (var renderer in bossRenderers)
                {
                    if (renderer != null) renderer.material.color = Color.red;
                }

                yield return new WaitForSeconds(hitFlashDuration);

                for (int i = 0; i < bossRenderers.Length; i++)
                {
                    if (bossRenderers[i] != null) bossRenderers[i].material.color = originalColors[i];
                }
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;

            Debug.Log("[BossController] Boss Defeated!");

            if (clearDetector != null)
            {
                clearDetector.UnregisterEnemy();
            }

            if (GameManager.Instance != null)
            {
                CrowdController crowd = FindFirstObjectByType<CrowdController>();
                int count = crowd != null ? crowd.TotalCount : 0;
                GameManager.Instance.OnBossDefeated(count);
            }

            Destroy(gameObject);
        }

        private void Update()
        {
            if (hpTextMesh != null && Camera.main != null)
            {
                hpTextMesh.transform.rotation = Quaternion.LookRotation(
                    hpTextMesh.transform.position - Camera.main.transform.position);
            }
        }
    }
}
