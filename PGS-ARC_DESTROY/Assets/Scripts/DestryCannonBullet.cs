using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryCannonBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroyTime");
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

  
}
