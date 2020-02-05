using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float cubeSize = 0.1f;
    public int cubesInRow = 5;
    public float explosionRadius = 10.0f;
    public float explosionForce = 20.0f;
    public float explosionUpward = 10.0f;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    protected float Animation;

    // Start is called before the first frame update
    void Start()
    {
        // calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    // Update is called once per frame
    void Update()
    {
        //
        Animation += Time.deltaTime;

        Animation = Animation % 3f;

        transform.position = MathParabola.Parabola(Vector3.zero , Vector3.forward * 3f , 3f , Animation / 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Bullet")
        {
            explode();
        }
    }

    public void explode()
    {
        gameObject.SetActive(false);

        for(int x=0; x<cubesInRow; x++)
        {
            for(int y=0; y<cubesInRow; y++)
            {
                for(int z=0; z<cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        //get explosion position
        Vector3 explosionPos = transform.position;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, explosionUpward);
            }
        }
    }

    void createPiece(int x , int y , int z)
    {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize , cubeSize , cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
    }
}
