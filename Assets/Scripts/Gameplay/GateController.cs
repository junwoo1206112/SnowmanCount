using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class GateController : MonoBehaviour
    {
        [Header("Gate Settings")]
        [SerializeField] private string operatorType = "+";
        [SerializeField] private int value = 5;
        [SerializeField] private Color positiveColor = Color.blue;
        [SerializeField] private Color negativeColor = Color.red;

        private CrowdController crowdController;
        private bool hasTriggered;
        private Collider gateCollider;
        private Renderer gateRenderer;
        private GameObject labelObject;

        private void Awake()
        {
            gateCollider = GetComponent<Collider>();
            gateRenderer = GetComponent<Renderer>();

            if (gateRenderer == null)
            {
                Debug.LogError("[GateController] No Renderer found on gate object");
            }
        }

        private void Start()
        {
            crowdController = FindFirstObjectByType<CrowdController>();

            if (crowdController == null)
            {
                Debug.LogError("[GateController] CrowdController not found in scene");
            }

            UpdateGateColor();
            CreateLabel();
        }

        private void CreateLabel()
        {
            labelObject = new GameObject("GateLabel");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = new Vector3(0f, 0f, -0.51f);

            TextMesh textMesh = labelObject.AddComponent<TextMesh>();
            textMesh.text = $"{operatorType}{value}";
            textMesh.fontSize = 80;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = Color.white;
            textMesh.characterSize = 0.08f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered)
            {
                return;
            }

            if (!other.CompareTag("Player"))
            {
                return;
            }

            hasTriggered = true;

            if (crowdController != null)
            {
                Debug.Log($"[GateController] Player passed gate: {operatorType}{value}");
                crowdController.ApplyMathOperation(operatorType, value);
            }

            DisableGate();
        }

        private void DisableGate()
        {
            if (gateCollider != null)
            {
                gateCollider.enabled = false;
            }

            if (gateRenderer != null)
            {
                gateRenderer.enabled = false;
            }

            if (labelObject != null)
            {
                labelObject.SetActive(false);
            }
        }

        public void SetGateData(string operation, int gateValue)
        {
            operatorType = operation;
            value = gateValue;
            UpdateGateColor();

            if (labelObject != null)
            {
                TextMesh textMesh = labelObject.GetComponent<TextMesh>();
                if (textMesh != null)
                {
                    textMesh.text = $"{operatorType}{value}";
                }
            }
        }

        private void UpdateGateColor()
        {
            if (gateRenderer == null) return;

            switch (operatorType)
            {
                case "+":
                case "x":
                case "*":
                    gateRenderer.material.color = positiveColor;
                    break;
                case "-":
                case "÷":
                case "/":
                    gateRenderer.material.color = negativeColor;
                    break;
                default:
                    gateRenderer.material.color = Color.gray;
                    break;
            }
        }
    }
}
