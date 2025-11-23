using UnityEngine;

public class QTETest : MonoBehaviour
{
    public Transform player; // Assign your player here
    public float qteDuration = 3f;

    private void Update()
    {
        // Press Q to trigger the QTE
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QTEManager.Instance.StartQTE(player, qteDuration, OnQTEResult);
        }
    }

    private void OnQTEResult(bool success)
    {
        if (success)
        {
            Debug.Log("QTE Success!");
        }
        else
        {
            Debug.Log("QTE Failed!");
        }
    }
}
