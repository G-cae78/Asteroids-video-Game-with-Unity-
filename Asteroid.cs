using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Asteroid : MonoBehaviour {
    // inspector settings
    public Rigidbody rigidBody; 
     public static Asteroid instance;
    public GameObject debris;
    public GameObject asteroid;
    public GameObject ship;


    public float cooldown=2f;
    public float lastHit;

    // Use this for initialization
    void Start () {
    
        transform.localScale = new Vector3(Random.Range(0.08f, 0.12f), Random.Range(0.08f, 0.12f), Random.Range(0.08f, 0.12f));
        rigidBody.mass = transform.localScale.x * transform.localScale.y * transform.localScale.z;

        // Randomize velocity
        rigidBody.linearVelocity = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        rigidBody.angularVelocity = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), Random.Range(-4f, 4f));

        // Start periodically checking for being off-screen
        InvokeRepeating("offScreen", 0.2f, 0.2f);
    }

    private void offScreen() {
        // Wrapping the asteroid around screen
        Vector3 pos = transform.position; 
        Vector3 vel = rigidBody.linearVelocity; 
        float xTeleport = 0f, zTeleport = 0f;

        // Check screen bounds and wrap asteroid around
        if (pos.x < GameManager.screenBottomLeft.x && vel.x <= 0f)
            xTeleport = GameManager.screenWidth;
        else if (pos.x > GameManager.screenTopRight.x && vel.x >= 0f)
            xTeleport = -GameManager.screenWidth;

        if (pos.z < GameManager.screenBottomLeft.z && vel.z <= 0f)
            zTeleport = GameManager.screenHeight;
        else if (pos.z > GameManager.screenTopRight.z && vel.z >= 0f)
            zTeleport = -GameManager.screenHeight;

        if (xTeleport != 0f || zTeleport != 0f)
            transform.position = new Vector3(pos.x + xTeleport, 0f, pos.z + zTeleport);
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Collided with: " + other.gameObject.tag);
        Vector3 collisionPoint = other.collider.ClosestPoint(transform.position);

        // Checking if asteroid has collided with player
        //setting cooldown to 3 seconds to prevent numerous respawning of spaceship 
        if (other.gameObject.CompareTag("Player") && Time.time>lastHit+cooldown) {
            GameManager.lives-=1;
            lastHit=Time.time; // getting last time space ship was hit
            Debug.Log("Collision with Player");
            Destroy(other.gameObject); // Destroying spaceship 
            Vector3 centre = new Vector3(0f, 0f, 0f); // Center of the screen
            Instantiate(ship, centre, Quaternion.Euler(90,0,0)); // Reinstantiating spaceship at center
        }
        
        // if (other.gameObject.CompareTag("asteroid")) {
        //     createDebris(collisionPoint); //if two asteroids collide create debris effects
        // }

        if (other.gameObject.CompareTag("bullet")) {
            GameManager.score+=250;
            Debug.Log("Shot by bullet");
            Debug.Log(rigidBody.mass);
            Destroy(other.gameObject);//destoy bullet after collision
            if (rigidBody.mass > 0.0005f) {
                // Destroy current asteroid and create two smaller asteroids
                if (GameManager.asteroids.Contains(this.gameObject)){
                GameManager.asteroids.Remove(this.gameObject);//Removing big asteroid from list and the destroying before splitting
                }
                Destroy(this.gameObject);
                splitLargeAsteroid();// calling method to split bigger asteroids
            }
             else{
               Debug.Log("Too small");
               //Removing asteroid from list and the destroying
                if (GameManager.asteroids.Contains(this.gameObject)){
                GameManager.asteroids.Remove(this.gameObject);
                }
               Debug.Log(GameManager.asteroids.Count);
               Destroy(this.gameObject);
               createDebris(collisionPoint);// calling method to spawn debris
            }
        }
        else{
            createDebris(collisionPoint);
        }

        // Log collision with any object
        Debug.Log("Collision with object");
    }

    void createDebris(Vector3 collisionPoint) {
        int numDebris = 6;//number of debris to spawn
        for (int i = 0; i < numDebris; i++) {
            Vector3 debrisRange = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            Vector3 spray = collisionPoint + debrisRange;//creating range of spray from collision point

            // Instantiate debris, set position and destroy after 0.7 seconds
            GameObject babyAsteroid = Instantiate(debris, spray, Quaternion.identity);
            Destroy(babyAsteroid, 0.7f);
        }
    }

    void splitLargeAsteroid() {
      Debug.Log("Splitting Asteroid");
        // Create two smaller asteroids at the same position as the current asteroid
        GameObject smallAsteroid1 = Instantiate(asteroid, transform.position, Quaternion.identity);
        GameObject smallAsteroid2 = Instantiate(asteroid, transform.position, Quaternion.identity);

        // Set new smaller scale for the asteroids
        smallAsteroid1.transform.localScale = transform.localScale * 0.6f; // 60% of original size
        smallAsteroid2.transform.localScale = transform.localScale * 0.4f; // 40% of original size

        // Distribute the mass proportionally to the size
        smallAsteroid1.GetComponent<Rigidbody>().mass = rigidBody.mass * 0.6f;
        smallAsteroid2.GetComponent<Rigidbody>().mass = rigidBody.mass * 0.4f;

        // Apply forces to move the smaller asteroids in opposite directions
       smallAsteroid1.GetComponent<Rigidbody>().linearVelocity = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
       smallAsteroid2.GetComponent<Rigidbody>().linearVelocity = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
       smallAsteroid1.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), Random.Range(-4f, 4f));
       smallAsteroid2.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), Random.Range(-4f, 4f));

       // Start the offScreen method for the smaller asteroids to handle wrapping
       smallAsteroid1.GetComponent<Asteroid>().InvokeRepeating("offScreen", 0.2f, 0.2f);
       smallAsteroid2.GetComponent<Asteroid>().InvokeRepeating("offScreen", 0.2f, 0.2f);

       GameManager.asteroids.Add(smallAsteroid1);
       GameManager.asteroids.Add(smallAsteroid2);

       Debug.Log(GameManager.asteroids.Count);

       Debug.Log(smallAsteroid1.GetComponent<Rigidbody>().mass);
       Debug.Log(smallAsteroid2.GetComponent<Rigidbody>().mass);
    }
}
