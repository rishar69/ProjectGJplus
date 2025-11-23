using UnityEngine;

public class MainCameraLookAhead : MonoBehaviour
{
    public Transform target;            
    public float followSmooth = 5f;     
    public float lookAheadDistance = 2f; 

    public Vector3 baseOffset = new Vector3(0, 0, -10);

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }


    private void FixedUpdate()
    {
        FollowCamera();
    }

    private void FollowCamera()
    {
        if (target == null) return;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direction = (mouseWorldPos - target.position).normalized;
        Vector3 lookAheadOffset = direction * lookAheadDistance;

        Vector3 desiredPosition = target.position + baseOffset + lookAheadOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmooth * Time.fixedDeltaTime);
    }
}
