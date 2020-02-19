using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float cubeSize = 1f;
    public int cubesInRow = 5;
    public float explosionRadius = 10.0f;
    public float explosionForce = 20.0f;
    public float explosionUpward = 10.0f;

    public GameObject ParticleEffect;

    float cubesPivotDistance;
    Vector3 cubesPivot;

   

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
        
      
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Head")
        {
            Debug.Log(other.gameObject.name);
            explode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TARGET")
        {
            
            explode();
            GameObject objR = Instantiate(ParticleEffect, this.transform.position, Quaternion.identity) as GameObject;
            Debug.Log("Boom!");
        }
    }

    public void explode()
    {
        gameObject.SetActive(false);

        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
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
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, explosionUpward);
            }
        }
    }

    void createPiece(int x, int y, int z)
    {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
        piece.AddComponent<DestroyExplodeCube>();
    }
}