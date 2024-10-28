using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ship;
    public Rigidbody rigidBody;
    void Start()
    {       
            Debug.Log(transform.forward);
            InvokeRepeating("offScreen",0f,0.2f);//runs every 0.2 seconds= 5 times per second
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void offScreen() {

    //getting current position of bullet and velocity
    Vector3 pos = transform.position; 
    Vector3 vel = rigidBody.linearVelocity; 
   

    //setting screen margins and destroying bullets if out of screen
   if (pos.x < GameManager.screenBottomLeft.x && vel.x <= 0f) // velocity check as sanity test 
      Destroy(this.gameObject);
   else if (pos.x > GameManager.screenTopRight.x && vel.x >= 0f) 
     Destroy(this.gameObject);
   if (pos.z < GameManager.screenBottomLeft.z && vel.z <= 0f) 
     Destroy(this.gameObject);
   else if (pos.z > GameManager.screenTopRight.z && vel.z >= 0f) 
     Destroy(this.gameObject);
     
}
}
