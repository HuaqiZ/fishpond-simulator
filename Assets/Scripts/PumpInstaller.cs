using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PumpInstaller : MonoBehaviour
{
    [SerializeField] private GameObject pumpObject = null;
    [SerializeField] private Transform waterObject = null;
    [SerializeField] private float nextDayWaterY = -0.5f;

    private bool isPumpInstalled;

    private void Start()
    {
        if (pumpObject != null)
        {
            pumpObject.SetActive(false);
        }
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
        {
            InstallPump();
        }
    }

    public void InstallPump()
    {
        if (isPumpInstalled)
        {
            return;
        }

        isPumpInstalled = true;

        if (pumpObject != null)
        {
            pumpObject.SetActive(true);
        }
    }

    public void ShowNextDayWaterLevel()
    {
        if (!isPumpInstalled || waterObject == null)
        {
            return;
        }

        Vector3 waterPosition = waterObject.position;
        waterPosition.y = nextDayWaterY;
        waterObject.position = waterPosition;
    }
}
