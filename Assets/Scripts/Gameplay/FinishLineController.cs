using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class FinishLineController : MonoBehaviour
    {
        private bool hasTriggered;
        private float roadWidth = 30f;

        public void SetRoadWidth(float width)
        {
            roadWidth = width;
        }

        private void Start()
        {
            BuildVisual();
            GetComponent<Collider>().isTrigger = true;
        }

        private void BuildVisual()
        {
            float span = roadWidth * 0.8f;

            GameObject groundLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
            groundLine.name = "FinishGroundLine";
            groundLine.transform.SetParent(transform);
            groundLine.transform.localPosition = new Vector3(0f, 0.05f, 0f);
            groundLine.transform.localScale = new Vector3(span, 0.05f, 0.5f);
            SetVisual(groundLine, new Color(1f, 0.85f, 0.1f));
            Destroy(groundLine.GetComponent<Collider>());

            GameObject glowBeam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            glowBeam.name = "FinishGlowBeam";
            glowBeam.transform.SetParent(transform);
            glowBeam.transform.localPosition = new Vector3(0f, 0.12f, 0f);
            glowBeam.transform.localScale = new Vector3(span + 0.5f, 0.08f, 0.7f);
            Renderer beamR = glowBeam.GetComponent<Renderer>();
            if (beamR != null)
            {
                beamR.material.color = new Color(1f, 1f, 0.4f);
                beamR.material.SetFloat("_Metallic", 0.9f);
            }
            Destroy(glowBeam.GetComponent<Collider>());

            float flagHeight = 1f;
            float poleWidth = 0.15f;
            float flagWidth = 0.5f;
            Color flagColor = new Color(1f, 0.5f, 0f);

            for (int side = -1; side <= 1; side += 2)
            {
                float xPos = side * (span * 0.5f + 0.4f);

                GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pole.name = side < 0 ? "FinishFlagPole_L" : "FinishFlagPole_R";
                pole.transform.SetParent(transform);
                pole.transform.localPosition = new Vector3(xPos, flagHeight * 0.5f, 0f);
                pole.transform.localScale = new Vector3(poleWidth, flagHeight, poleWidth);
                SetVisual(pole, new Color(0.8f, 0.8f, 0.8f));

                GameObject flag = GameObject.CreatePrimitive(PrimitiveType.Cube);
                flag.name = side < 0 ? "FinishFlag_L" : "FinishFlag_R";
                flag.transform.SetParent(transform);
                flag.transform.localPosition = new Vector3(xPos + side * flagWidth * 0.4f, flagHeight, 0f);
                flag.transform.localScale = new Vector3(flagWidth, 0.3f, 0.05f);
                SetVisual(flag, flagColor);
            }
        }

        private void SetVisual(GameObject obj, Color color)
        {
            Renderer r = obj.GetComponent<Renderer>();
            if (r != null)
            {
                r.material.color = color;
                r.material.SetFloat("_Metallic", 0.3f);
            }
            Destroy(obj.GetComponent<Collider>());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;
            if (!other.CompareTag("Player")) return;

            hasTriggered = true;
            Debug.Log("[FinishLineController] Player reached finish line!");

            EnemyGroup[] groups = FindObjectsByType<EnemyGroup>(FindObjectsSortMode.None);
            foreach (var g in groups) Destroy(g.gameObject);

            EnemyMinion[] minions = FindObjectsByType<EnemyMinion>(FindObjectsSortMode.None);
            foreach (var m in minions) Destroy(m.gameObject);

            ObstacleController[] obstacles = FindObjectsByType<ObstacleController>(FindObjectsSortMode.None);
            foreach (var o in obstacles) Destroy(o.gameObject);

            BossController[] bosses = FindObjectsByType<BossController>(FindObjectsSortMode.None);
            foreach (var b in bosses) Destroy(b.gameObject);

            CrowdController crowd = FindFirstObjectByType<CrowdController>();
            int crowdCount = crowd != null ? crowd.TotalCount : -1;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLevelCleared(crowdCount);
            }
        }
    }
}
