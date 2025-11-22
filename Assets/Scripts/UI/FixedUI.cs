using UnityEngine;

public class FixedUI : MonoBehaviour
{
    private Quaternion originalRotation;

    void Awake()
    {
        // Save the rotation you want the UI to keep forever
        originalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // 1. Prevent rotation from parent
        transform.rotation = originalRotation;

        // 2. Prevent flipping from parent scale
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
