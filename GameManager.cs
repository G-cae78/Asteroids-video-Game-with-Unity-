//using System;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // inspector settings
    public GameObject asteroidPrefab; //
    // class-level statics
    public static GameManager instance;
    public static int currentGameLevel;
    public static Vector3 screenBottomLeft, screenTopRight; 
    public static float screenWidth, screenHeight;
    public static List<GameObject> asteroids;
    public GameObject ship;

    //public List<Asteroid> asteroids;
    public TMP_Text displayScore;
    public TMP_Text displayHighScore;
     public TMP_Text displayLives;
    public static int score=0;
    public static int highScore=0;
    public static int lives=3;
    

    public bool inMenu=true;
    public bool playing=false;

    void Start() {
      //StartNewGame();
    }
    void Awake() {
    if (instance == null) {
        instance = this;
        //StartNewGame();
       // DontDestroyOnLoad(gameObject);
    } else {
        Destroy(gameObject); // Ensures only one instance of GameManager exists
    }
   }

   void FixedUpdate(){
      if(score>highScore)
      highScore=score;

      displayScore.text="Score: "+GameManager.score.ToString();
      displayHighScore.text="High Score: "+GameManager.highScore.ToString(); 
      if(lives==0){
        SceneManager.LoadScene("Menu",LoadSceneMode.Single);
      }
      displayLives.text="Lives: "+GameManager.lives.ToString();
   }
    void Update() {
        Debug.Log(asteroids.Count);
      if(!asteroids.Any()){
         StartNextLevel();
      }
      //Destroying asteroids left over before new game starts or game ends
      if(inMenu){
        foreach(GameObject asteroid in asteroids){
            Destroy(asteroid);
        }
        asteroids.Clear();
      }
   }

    
    public void StartNewGame(){
        Debug.Log("Starting game");
        instance = this;
        asteroids= new List<GameObject>();
        Camera.main.transform.position = new Vector3(0f, 30f, 0f); 
        Camera.main.transform.LookAt(Vector3.zero, new Vector3(0f, 0f, 1f)); 
        currentGameLevel = 0;

        screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 30f)); 
        screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 30f)); 
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.z - screenBottomLeft.z;

        CreatePlayerShip();// calling method to instntiate spaceship object
        StartNextLevel(); // calling method to start next level
    }
    public void StartButton(){
        Debug.Log("Start Button Pressed");
        this.inMenu=false;
        this.playing=true;
         if(playing && !inMenu){
            SceneManager.LoadScene("ScoreBoard");
            StartNewGame();
        }
        else{
            inMenu=true;
        }
        //LoadGame();
    }


    public static void StartNextLevel() { 
        currentGameLevel++;

        // create some asteroids near the edges of the screen
        for (int i = 0; i < currentGameLevel * 2 + 1; i++) {
            GameObject go = Instantiate(instance.asteroidPrefab) as GameObject; 
            asteroids.Add(go);
            float x, z;
            if (Random.Range(0f, 1f) < 0.5f)
                x = screenBottomLeft.x + Random.Range(0f, 0.15f) * screenWidth; 
            else
                x = screenTopRight.x - Random.Range(0f, 0.15f) * screenWidth; 
            if (Random.Range(0f, 1f) < 0.5f)
                z = screenBottomLeft.z + Random.Range(0f, 0.15f) * screenHeight; 
            else
                z = screenTopRight.z - Random.Range(0f, 0.15f) * screenHeight; 

            go.transform.position = new Vector3(x, 0f, z);
        }
       Debug.Log(asteroids.Count);
    }

    public void CreatePlayerShip() {
        //instatiating spaceShip object
        Vector3 shipPosition = new Vector3(0f, 0f, 0f); // Center of the screen
        Instantiate(ship, shipPosition, Quaternion.Euler(90,0,0)); 
    }
}
