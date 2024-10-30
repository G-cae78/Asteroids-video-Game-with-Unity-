using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
//using System.Numerics;
using UnityEngine;

public class SpaceShipScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ship;
    public Rigidbody rigid;
    public GameObject bullet;
     private float bulletCooldown = 0.25f; // Cooldown duration in seconds
    private float LastFired;
    
    void Start()
    {
         rigid = GetComponent<Rigidbody>();
         InvokeRepeating("offScreen",0f,0.2f);//runs every 0.2 seconds= 5 times per second
         
    }

    // Update is called once per frame
    
     void FixedUpdate () {
        if (Input.GetKey(KeyCode.UpArrow))
            rigid.AddForce(transform.up * (rigid.mass * Time.fixedDeltaTime * 2000f));
        else if (Input.GetKey(KeyCode.UpArrow))
            rigid.AddForce(-transform.up * (rigid.mass * Time.fixedDeltaTime * 2000f));
        // we're using an Angular Drag of 15.0 on the rigid body, so need a lot of torque here
        if (Input.GetKey(KeyCode.LeftArrow))
          rigid.AddTorque(-Vector3.up * (rigid.mass * Time.fixedDeltaTime * 4000f));
        else if (Input.GetKey(KeyCode.RightArrow))
            rigid.AddTorque(Vector3.up * (rigid.mass * Time.fixedDeltaTime * 4000f));
        else if (Input.GetKey(KeyCode.RightArrow))
            rigid.AddTorque(Vector3.up * (rigid.mass * Time.fixedDeltaTime * 4000f));
        
        //setting a cooldown to fire bullets at 4 times per second limit by firing one bullet every 0.25 milliseconds
         else if (Input.GetKey(KeyCode.Space)&& Time.time >= LastFired+bulletCooldown)//bullet fired when space bar is pressed
        {
            LastFired=Time.time;//updating time of when last bullet was fired
            Vector3 bulletPos = transform.position + transform.up * 2f;
            GameObject bull = Instantiate(bullet, bulletPos, transform.rotation);//instantiating and setting bullet position
            bull.GetComponent<Rigidbody>().linearVelocity=ship.transform.up*20f;//making bullet move forward in the direction the ship is facing
            Debug.Log(bull.GetComponent<Rigidbody>().linearVelocity);
          //  bull.transform.rotation=ship.transform.rotation;
           
        }
    }
        
 void offScreen() {

//getting ships positin and current velocity
    Vector3 pos = ship.transform.position; 
    Vector3 vel = rigid.linearVelocity; 
    float xTeleport = 0f, zTeleport = 0f;

//setting screen margins and making sure ship stays in bounds, if not wrap around
   if (pos.x < GameManager.screenBottomLeft.x && vel.x <= 0f) // velocity check as sanity test 
       xTeleport = GameManager.screenWidth;
   else if (pos.x > GameManager.screenTopRight.x && vel.x >= 0f) 
      xTeleport = -GameManager.screenWidth;
   if (pos.z < GameManager.screenBottomLeft.z && vel.z <= 0f) 
      zTeleport = GameManager.screenHeight;
   else if (pos.z > GameManager.screenTopRight.z && vel.z >= 0f) 
      zTeleport = -GameManager.screenHeight;
      //finally wrapping ship to new position on screen
   if (xTeleport != 0f || zTeleport != 0f)
      ship.transform.position = new Vector3 (pos.x + xTeleport, 0f, pos.z + zTeleport);
}
}