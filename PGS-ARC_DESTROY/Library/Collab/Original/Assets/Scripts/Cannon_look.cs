using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_look : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public GameObject Obj;
    public Transform shootPos;
    public float shootSpeed;
    public bool shootBool;
    public int shootInt = 0;

    void Start()
    {
        shootBool = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shootBool==true && shootInt == 0)
        {
            
            StartCoroutine("DelayShoot");
            shootInt = 1;
           
        }
        
        
    }

    IEnumerator DelayShoot()
    {
        transform.LookAt(target);
        yield return new WaitForSeconds(Random.Range(3f,4f));
       
        Vector3 Vo = CalculateVelocity(target.position, shootPos.position, 1f);

        if (shootInt==1 && shootBool)
        {
            shootInt = 1;
            shootBool = false;
            transform.LookAt(target);
            GameObject objR = Instantiate(Obj, shootPos.position, Quaternion.identity) as GameObject;            
            objR.GetComponent<Rigidbody>().velocity = Vo;
            
            

        }

        
        
        

        
        shootBool = false;

        StartCoroutine("ShootDelay");
        
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.5f);
        
        shootBool = true;
        shootInt = 0;

    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + (0.5f * Mathf.Abs(Physics.gravity.y)*time);

        Vector3 result = distance.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }


}
