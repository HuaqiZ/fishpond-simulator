using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class WaterLevelController : MonoBehaviour
{
    [Header("Drain")]
    [SerializeField] private bool autoStart;
    [SerializeField] private KeyCode toggleKey = KeyCode.P;
    [SerializeField, Min(0.01f)] private float drainSpeed = 0.25f;
    [SerializeField] private float targetWaterLevelY = -0.4f;
    [SerializeField, Min(0.01f)] private float minVisibleHeight = 0.03f;
    [SerializeField] private bool shrinkCubeFromBottom = true;

    [Header("Debug")]
    [SerializeField] private bool isDraining;
    [SerializeField] private float currentWaterLevelY;

    public UnityEvent<float> OnWaterLevelChanged;

    private Vector3 startLocalScale;
    private Vector3 startWorldPosition;
    private float startWaterLevelY;
    private float bottomLevelY;

    public bool IsDraining => isDraining;
    public float CurrentWaterLevelY => currentWaterLevelY;

    public float NormalizedRemaining
    {
        get
        {
            float range = Mathf.Max(0.001f, startWaterLevelY - targetWaterLevelY);
            return Mathf.Clamp01((currentWaterLevelY - targetWaterLevelY) / range);
        }
    }

    private void Awake()
    {
        CacheStartState();
        currentWaterLevelY = startWaterLevelY;
        isDraining = autoStart;
        ApplyWaterLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDrain();
        }

        if (!isDraining)
        {
            return;
        }

        currentWaterLevelY = Mathf.MoveTowards(
            currentWaterLevelY,
            targetWaterLevelY,
            drainSpeed * Time.deltaTime
        );

        ApplyWaterLevel();

        if (Mathf.Approximately(currentWaterLevelY, targetWaterLevelY))
        {
            isDraining = false;
        }
    }

    public void StartDraining()
    {
        isDraining = true;
    }

    public void StopDraining()
    {
        isDraining = false;
    }

    public void ToggleDrain()
    {
        isDraining = !isDraining;
    }

    public void ResetWater()
    {
        isDraining = false;
        currentWaterLevelY = startWaterLevelY;
        transform.localScale = startLocalScale;
        transform.position = startWorldPosition;
        OnWaterLevelChanged?.Invoke(currentWaterLevelY);
    }

    public void SetTargetWaterLevel(float waterLevelY)
    {
        targetWaterLevelY = Mathf.Clamp(waterLevelY, bottomLevelY + minVisibleHeight, startWaterLevelY);
    }

    private void CacheStartState()
    {
        startLocalScale = transform.localScale;
        startWorldPosition = transform.position;

        float halfHeight = Mathf.Abs(transform.lossyScale.y) * 0.5f;
        startWaterLevelY = startWorldPosition.y + halfHeight;
        bottomLevelY = startWorldPosition.y - halfHeight;
        targetWaterLevelY = Mathf.Clamp(targetWaterLevelY, bottomLevelY + minVisibleHeight, startWaterLevelY);
    }

    private void ApplyWaterLevel()
    {
        if (shrinkCubeFromBottom)
        {
            float startHeight = Mathf.Max(0.001f, startWaterLevelY - bottomLevelY);
            float height = Mathf.Max(minVisibleHeight, currentWaterLevelY - bottomLevelY);
            float heightRatio = height / startHeight;

            Vector3 newScale = startLocalScale;
            newScale.y = startLocalScale.y * heightRatio;
            transform.localScale = newScale;

            Vector3 newPosition = transform.position;
            newPosition.y = bottomLevelY + height * 0.5f;
            transform.position = newPosition;
        }
        else
        {
            Vector3 newPosition = startWorldPosition;
            newPosition.y = startWorldPosition.y - (startWaterLevelY - currentWaterLevelY);
            transform.position = newPosition;
        }

        OnWaterLevelChanged?.Invoke(currentWaterLevelY);
    }
}

