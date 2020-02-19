using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ATMCam : MonoBehaviour
{

    public Transform target;
    Vector2 pointerStartPos;
    Vector2 pointerCurrentPos;

    public float fovMin = 30;
    public float fovMax = 70;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerStartPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            pointerCurrentPos = Input.mousePosition;
            
            float directionX = pointerCurrentPos.x - pointerStartPos.x;
            float directionY = pointerCurrentPos.y - pointerStartPos.y;
           
           

            if (directionX > directionY)
            {
                Debug.Log("X:"+directionX);
                transform.RotateAround(target.transform.position, Vector3.up, 0.5f * directionX / 10f);
            }
            else
            {
                Debug.Log("Y:"+directionY);
                transform.RotateAround(target.transform.position, Vector3.right, 0.5f * directionX / 10f);
               // transform.position = new Vector3(transform.position.x, transform.position.y + directionY, transform.position.z);
            }
          
        }



        if (Input.touchCount == 2)
        {

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            zoom(difference);
        }

    }
    void zoom(float increment)
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment, fovMin, fovMax);
    }

}
