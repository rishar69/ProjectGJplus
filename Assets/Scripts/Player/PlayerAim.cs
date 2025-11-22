using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Transform aimTransform;


    private void Awake()
    {
        aimTransform = transform.Find("Aim");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
  
    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        worldPos.z = 0f;

        return worldPos;
    }


    private void AimingHandler()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Rotate the aim object
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        //Debug.Log(angle);

        // Rotate the parent on the Y-axis (3D flip)
        if (angle > 90 || angle < -90)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log("fliped");
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    // Update is called once per frame
    void Update()
    {
        AimingHandler();

    }






}



