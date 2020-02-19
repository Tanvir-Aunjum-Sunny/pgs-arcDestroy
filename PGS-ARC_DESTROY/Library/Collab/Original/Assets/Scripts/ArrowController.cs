using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // register collision
    bool collisionOccurred;

    // References to GameObjects gset in the inspector
    public GameObject arrowHead;
    public GameObject bow;


    // the vars realize the fading out of the arrow when target is hit
    float alpha;
    float life_loss;
    public Color color = Color.white;

    GameObject ground;
   

    // Use this for initialization
    void Start()
    {
        // set the initialization values for fading out
        float duration = 2f;
        life_loss = 1f / duration;
        alpha = 1f;

        ground = GameObject.FindGameObjectWithTag("Ground");
    }



    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5f)
        {
            // create new arrow
            bow.GetComponent<BowController>().createArrow();
            Destroy(gameObject);
        }

        //this part of update is only executed, if a rigidbody is present
        // the rigidbody is added when the arrow is shot (released from the bowstring)
        if (transform.GetComponent<Rigidbody>() != null)
        {
            // do we fly actually?
            if (GetComponent<Rigidbody>().velocity != Vector3.zero)
            {
                // get the actual velocity
                Vector3 vel = GetComponent<Rigidbody>().velocity;
                // calc the rotation from x and y velocity via a simple atan2
                float angleZ = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                float angleY = Mathf.Atan2(vel.z, vel.x) * Mathf.Rad2Deg;
                // rotate the arrow according to the trajectory
               // transform.eulerAngles = new Vector3(0, -angleY, angleZ);
            }
        }

        // if the arrow hit something...
        if (collisionOccurred)
        {
            // fade the arrow out
            alpha -= Time.deltaTime * life_loss;
            GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, alpha);

            // if completely faded out, die:
            if (alpha <= 0f)
            {
                // create new arrow
                bow.GetComponent<BowController>().createArrow();
                // and destroy the current one
                Destroy(gameObject);
            }
        }

    }

    void OnCollisionEnter(Collision other)
    {


        //so, did a collision occur already?
        if (collisionOccurred)
        {
            // fix the arrow and let it not move anymore
           // transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
            // the rest of the method hasn't to be calculated
           // return;
        }

        // I installed cubes as border collider outside the screen
        // If the arrow hits these objects, the player lost an arrow
        Debug.Log(other.transform.name);
        if (other.transform.tag == "Ground")
        {
            Destroy(gameObject);
            bow.GetComponent<BowController>().createArrow();
           
        }

        // Ok - 
        // we hit the target
        if (other.transform.name == "target")
        {
            // play the audio file ("trrrrr")
           
            // set velocity to zero
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            // disable the rigidbody
            GetComponent<Rigidbody>().isKinematic = true;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // and a collision occurred
            collisionOccurred = true;
            // disable the arrow head to create optical illusion
            // that arrow hit the target
            arrowHead.SetActive(false);



           
        }
    }

    //
    // public void setBow
    //
    // set a reference to the main game object 

    public void setBow(GameObject _bow)
    {
        bow = _bow;
    }

}
