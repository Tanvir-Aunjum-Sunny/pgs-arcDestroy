using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask layer;
    public Transform shootPoint;
    private Camera cam;

    public GameObject target;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        TankMovement();
    }


    void TankMovement()
    {
        /* Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;
         if (Physics.Raycast(camRay, out hit, 100f,layer))
         {
             Vector3 target_cannon = CalculateVelocity(hit.point, shootPoint.position, 1f);
             transform.rotation = Quaternion.LookRotation(target_cannon);


             if (Input.GetMouseButtonDown(0))
             {
                 //
             }

         }*/
        Vector3 target_cannon = CalculateVelocity(target.transform.position, shootPoint.position, 1f);
        transform.rotation = Quaternion.LookRotation(target_cannon);


    }
    
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time) 
    {
        //distance of horizontal and vertical
        Vector3 distanceFromTarget = target - origin;
        Vector3 distanceXZ = distanceFromTarget;
        distanceXZ.y = 0f;

        // distance calculation
        float distance_horizontal = distanceFromTarget.y;
        float distance_vertical = distanceXZ.magnitude;

        float velocity_horizontal = distance_horizontal / time;
        float velocity_vertical = distance_vertical / time + 0.5f * Mathf.Abs(Physics.gravity.y)*time;

        Vector3 result_normalized = distanceXZ.normalized;
        result_normalized *= velocity_horizontal;
        result_normalized.y = -velocity_vertical;
        Debug.Log(result_normalized.y);

        return result_normalized;
    }
}
