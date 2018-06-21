using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**GameManager Class
//* Handles all Game-related scirpting.
//* Score, Stage RNG, Stage Transitions, etc. 

public class GameManager : MonoBehaviour {

    //Stage Variables
    [Header("Stage Variables")]
    public GameObject[] stages;             //Array of stage-types
    private GameObject currentStage;        //Currently selected stage
    private bool stageActive = false;       //Bool check if a stage is already chosen
    private int rand = 1;                   //Randon number used pick a stage in the array (Initialized to 0 to always start on first level)
    private int currentInt = -1;            //To be compared with the random number to prevent duplicates
    private int bossInt = 0;                //Counter to ensure the boss stage can be chosen after set amount of stages

    //Player Variables                      ***Should be moved to the Player class
    [Header("Player Variables")]
    public GameObject player;
    public Transform playerSpawn;

    //Fade Variables
    [Header("Fade Variables")]
    public Texture2D fadeOutTexture;        //Texture for the Fade
    public float fadeSpeed = 0.8f;          //The speed in which the fade occurs

    private int drawDepth = -1000;          //Depth is set to always draw on top
    private float alpha = 1.0f;             //Alpha value of the texture
    private int fadeDir = -1;               //Fade direction determines whether texture is fading IN or OUT

    //Music Variables
    public AudioSource audioSource;         //Audio for when the player is fighting a boss

    void Awake ()
    {
        // Create player
        player = Instantiate(player, playerSpawn.position, playerSpawn.rotation);
        player.name = "Player";
        //Generate a random Map which the scene loads
        GenerateTileMap();
    }
	
	void Update ()
    {
        if(GameObject.FindWithTag("Boss") != null && !player.GetComponent<Player>().dead)
        {
            audioSource.volume += 0.2f * Time.deltaTime;
        }
        else
        {
            audioSource.volume -= 0.2f * Time.deltaTime;
        }
	}

    public void GenerateTileMap()
    {
        //If a map is already active then destroy it first
        if (stageActive)
            DestroyTileMap();

        //Increment bossInt to increase chances of spawning the boss stage
        bossInt++;

        //If the random number and currently chosen stage have the same number, reroll the number (Prevent back-to-back duplicate stages)
        while (currentInt == rand)
        {
            if (bossInt <= 5)
                rand = Random.Range(0, 8);              //Unable to encounter boss
            else if (bossInt >= 6 || bossInt < 8)
                rand = Random.Range(5, 9);              //Chance to encounter boss
            else
                rand = 8;                               //Guarantee boss after 8 stages
        }

        //Generate a new map based on a random number
        currentStage = Instantiate(stages[rand]);
        stageActive = true;
        currentInt = rand;

        //Reset bossInt when a boss has been encountered
        if (rand == 8)
            bossInt = 0;

        //Move player to the spawn
        player.transform.position = playerSpawn.position;

        //Fade the GUI texture OUT
        BeginFade(-1);
    }

    void DestroyTileMap()
    {
        Destroy(currentStage);
        stageActive = false;
    }

    void OnGUI()
    {
        //For testing only!
        //if (GUI.Button(new Rect(0, 0, 100, 20), "Generate"))
        //{
        //    GenerateTileMap();
        //}

        //Setting up the fade texture and altering its alpha value
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public IEnumerator NextLevel()
    {
        //When the stage is complete then fade IN the texture and load in a new stage prefab
        BeginFade(1);
        yield return new WaitForSeconds(fadeSpeed);
        GenerateTileMap();
    }

    public void BeginFade(int direction)
    {
        //Depending on the direction passed, the texture will fade IN or OUT
        fadeDir = direction;
    }
}
