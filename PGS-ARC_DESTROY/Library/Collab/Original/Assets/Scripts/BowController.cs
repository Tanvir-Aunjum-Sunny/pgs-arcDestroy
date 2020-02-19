using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    // to determine the mouse position, we need a raycast
    private Ray mouseRay1;
    private RaycastHit rayHit;
    // position of the raycast on the screen
    private float posX;
    private float posY;

    // References to the gameobjects / prefabs
    [Header("Arrow and String")]
    public GameObject bowString;
    public GameObject arrowPrefab;
    //public GameObject target;
    // amount of arrows for the game
    public int arrows = 20;
    GameObject arrow;

    // the bowstring is a line renderer
    private List<Vector3> bowStringPosition;
    LineRenderer bowStringLinerenderer;

    // to determine the string pullout
    float arrowStartX;
    float length;

    // some status vars
    bool arrowShot;
    bool arrowPrepared;

    // position of the line renderers middle part 
    Vector3 stringPullout;
    Vector3 stringRestPosition = new Vector3(-0.23f, 0.04f, 0);



    //Trajectory Dots
    [Header("Trajectory dots")]
    public GameObject trajectoryDot;
    public int number;
    public Transform trajectoryDotStartPos;
    public Transform trajectoryDotEndPos;
    private GameObject[] trajectoryDots;
    



    void Start()
    {

        // create an arrow to shoot
        // use true to set the target
        createArrow();

        // setup the line renderer representing the bowstring
        bowStringLinerenderer = bowString.AddComponent<LineRenderer>();
        bowStringLinerenderer.SetVertexCount(3);
        bowStringLinerenderer.SetWidth(0.05F, 0.05F);
        bowStringLinerenderer.useWorldSpace = false;
        bowStringLinerenderer.material = Resources.Load("Materials/bowStringMaterial") as Material;
        bowStringPosition = new List<Vector3>();
        bowStringPosition.Add(new Vector3(-0.11f, 1.29f, 0));
        bowStringPosition.Add(new Vector3(-0.23f, 0.04f, 0));
        bowStringPosition.Add(new Vector3(-0.23f, -1f, 0));
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, bowStringPosition[1]);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
        arrowStartX = 0.7f;

        stringPullout = stringRestPosition;

       

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        { 
            
            for (int i = 0; i < number; i++)
            {
               // trajectoryDots[i] = Instantiate(trajectoryDot, gameObject.transform);
            }

        }
        if (Input.GetMouseButton(0))
        {
           
            // detrmine the pullout and set up the arrow
            prepareArrow();
            for (int i = 0; i < number; i++)
            {
              //  trajectoryDots[i].transform.position = calculatePosition(i * 0.1f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            stringPullout = stringRestPosition;
            // shot the arrow (rigid body physics)
            shootArrow();
            //Destroy trajectory points
            for (int i = 0; i < number; i++)
            {
                Destroy(trajectoryDots[i]);
            }
        }
        // in any case: update the bowstring line renderer
        drawBowString();


        if (this.transform.rotation.z > 80)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 80f));
        }
        else if(this.transform.rotation.z < -70f)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -70f));
        }
    }


    // this method creates a new arrow based on the prefab
    public void createArrow()
    {
       // Camera.main.GetComponent<camMovement>().resetCamera();

        // does the player has an arrow left ?
        if (arrows > 0)
        {
           
            // now instantiate a new arrow
            this.transform.localRotation = Quaternion.identity;
            arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            
            arrow.name = "Arrow";
            arrow.transform.localScale = new Vector3(0.15f, 1, 0.15f);
            arrow.transform.localPosition = this.transform.position + new Vector3(0.7f, 0.155f, 0);
            arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            arrow.transform.parent = this.transform;
            // transmit a reference to the arrow script
            arrow.GetComponent<ArrowController>().setBow(gameObject);
            arrowShot = false;
            arrowPrepared = false;
            // subtract one arrow
            arrows--;
        }
       
    }


    // 
    // public void prepareArrow()
    //
    // Player pulls out the string
    //

    public void prepareArrow()
    {
        // get the touch point on the screen
        mouseRay1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay1, out rayHit, 1000f) && arrowShot == false)
        {
            // determine the position on the screen
            posX = this.rayHit.point.x;
            posY = this.rayHit.point.y;
            // set the bows angle to the arrow
            Vector2 mousePos = new Vector2(transform.position.x - posX, transform.position.y - posY);
            float angleZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angleZ);
            // determine the arrow pullout
            length = mousePos.magnitude / 3f;
            length = Mathf.Clamp(length, 0, 1);
            // set the bowstrings line renderer
            stringPullout = new Vector3(-length, 0, 0);
            // set the arrows position
            Vector3 arrowPosition = arrow.transform.localPosition;
            arrowPosition.x = (arrowStartX - length);
            arrow.transform.localPosition = arrowPosition;
            Debug.Log(arrowPosition);
        }
        arrowPrepared = true;
    }

    //
    // public void shootArrow()
    //
    // Player released the arrow
    // get the bows rotationn and accelerate the arrow
    //

    public void shootArrow()
    {
        if (arrow.GetComponent<Rigidbody>() == null)
        {
            arrowShot = true;
            arrow.AddComponent<Rigidbody>();       
            arrow.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)) * new Vector3(25f * length, 0, 0), ForceMode.VelocityChange);
        }
        arrowPrepared = false;
        stringPullout = stringRestPosition;

        // Cam
        

    }

    public void drawBowString()
    {
        bowStringLinerenderer = bowString.GetComponent<LineRenderer>();
        bowStringLinerenderer.SetPosition(0, bowStringPosition[0]);
        bowStringLinerenderer.SetPosition(1, stringPullout);
        bowStringLinerenderer.SetPosition(2, bowStringPosition[2]);
    }



}
