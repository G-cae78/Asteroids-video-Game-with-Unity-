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
    //public TMP_Text scoreBoard;

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
      LoadHighScore();
    }
    void Awake() {
    //Using DontDestroy to prevent game object from being deleted when switching between scenes
    DontDestroyOnLoad(gameObject);
    DontDestroyOnLoad(asteroidPrefab);
    DontDestroyOnLoad(ship);
    //DontDestroyOnLoad(scoreBoard);
}


   void FixedUpdate(){

      if(score>highScore)//updating high score if necessary
      highScore=score;
      SaveHighScore();

      //displaying score board text 
      displayScore.text="Score: "+GameManager.score.ToString();
      displayHighScore.text="High Score: "+GameManager.highScore.ToString(); 

      if(lives==0){
        SceneManager.LoadScene("Menu",LoadSceneMode.Single);
      }
      //displaying player lives on score board
      displayLives.text="Lives: "+GameManager.lives.ToString();
   }
    void Update() {
        //Debug.Log(asteroids.Count);
      if(!asteroids.Any()){//Checking if there are still asteroids left to be destroyed
         StartNextLevel();//starting new level if all asteroids have been destroyed
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
        asteroids= new List<GameObject>();//initialising list of asteroid objects
        Camera.main.transform.position = new Vector3(0f, 30f, 0f); 
        Camera.main.transform.LookAt(Vector3.zero, new Vector3(0f, 0f, 1f)); 
        currentGameLevel = 0;

        //Setting camera view point
        screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 30f)); 
        screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 30f)); 
        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.z - screenBottomLeft.z;

        CreatePlayerShip();// calling method to instntiate spaceship object
        StartNextLevel(); // calling method to start next level
    }
    //Function to be called when start button is clicked
   public void StartButton() {
    Debug.Log("Start Button Pressed");
    //changing game state to playing when start button is pressed
    this.inMenu = false;
    this.playing = true;

    if (playing && !inMenu) {
        //Using couroutine to ensure scene is fully loaded before starting game
        StartCoroutine(LoadScoreBoardScene());
    }
}

private IEnumerator LoadScoreBoardScene() {
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ScoreBoard");
    while (!asyncLoad.isDone) {
        yield return null; // wait for the scene to load
    }
    StartNewGame(); // Then start the new game
}



    public static void StartNextLevel() { 
        // score=0;//resetting current score every new game level
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
    void SaveHighScore(){
        PlayerPrefs.SetInt("HighScore",highScore);//Using unity PlayerPrefs to store high score between games
    }
    void LoadHighScore(){
        Debug.Log(PlayerPrefs.GetInt("HighScore"));
        highScore= PlayerPrefs.GetInt("HighScore");// retrieving players highscore from player history
    }
}
