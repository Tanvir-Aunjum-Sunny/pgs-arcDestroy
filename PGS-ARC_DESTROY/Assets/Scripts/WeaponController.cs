using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    public Vector3 initPos;
    private Rigidbody weaponRigidBody;
    private Vector3 forceAtPlayer;
    public float forceFactor;

    public GameObject trajectoryDot;
    private GameObject[] trajectoryDots;
    public int number;

    

    // Start is called before the first frame update
    void Start()
    {
        weaponRigidBody = GetComponent<Rigidbody>();
        trajectoryDots = new GameObject[number];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { //click
            startPos = gameObject.transform.position;
            for (int i = 0; i < number; i++)
            {
                trajectoryDots[i] = Instantiate(trajectoryDot, gameObject.transform);
            }

        }
        if (Input.GetMouseButton(0))
        { //drag
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(10f, 0, 0);
            gameObject.transform.position = endPos;
            forceAtPlayer = endPos - startPos;
            for (int i = 0; i < number; i++)
            {
                trajectoryDots[i].transform.position = calculatePosition(i * 0.1f);
            }
        }
        if (Input.GetMouseButtonUp(0))
        { //leave
            //rigidbody.gra = 1;
            weaponRigidBody.velocity = new Vector3(-forceAtPlayer.x * forceFactor, -forceAtPlayer.y * forceFactor, 0f);
            for (int i = 0; i < number; i++)
            {
                Destroy(trajectoryDots[i]);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //rigidbody.gravityScale = 0;
            weaponRigidBody.velocity = Vector2.zero;
            gameObject.transform.position = initPos;

        }
    }

    private Vector3 calculatePosition(float elapsedTime)
    {
        return new Vector3(endPos.x, endPos.y,0f) + //X0
                new Vector3(-forceAtPlayer.x * forceFactor, -forceAtPlayer.y * forceFactor,0f) * elapsedTime + //ut
                0.5f * Physics.gravity * elapsedTime * elapsedTime;
    }
}
