using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Score
{
    public int highScore = 10;
}


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private MenuHandler menuHandler;
    [SerializeField] float horizontalMovementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] TMP_Text currentScoreText, highScoreText;
    [SerializeField] AudioClip fallSound, bounceSound;
    [SerializeField] AudioSource audioSource;
    private float horizontalMovement;
    private Rigidbody2D rb;
    private Transform cameraTransform;
    private float boundaryDistanceVertical;
    private float boundaryDistanceHorizontal;
    private float highestY;
    private string saveFilePath;

    Animator anim;

    private int score;

    public int Score
    {
        get { return score; } //gets the return value of the bool of score
        set
        {
            score = value; //value of score is set to "value"
            currentScoreText.text = value.ToString(); //changes the score text
        }
    }
    private Score highScore;

    private bool dead;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        anim = GetComponent<Animator>(); //Gets the animation from the animator

        saveFilePath = Application.dataPath + "/HighScore.bin"; //save file in applications and into dataPath with the name "HighScore.bin"
        LoadHighScore(); //Do LoadHighScore Method
        rb = GetComponent<Rigidbody2D>(); //Getting the RigidBody Component
    }
    // Start is called before the first frame update
    void Start()
    {
        //Gives the values from Camera.main.transform to cameraTransfrom
        cameraTransform = Camera.main.transform;
        //making the distance boundry to equal the orthograpghic size proportional to the width/height of screen
        boundaryDistanceHorizontal = cameraTransform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height;
        //making the distance boundry to equal the orthograpghic 
        boundaryDistanceVertical = Camera.main.orthographicSize + 0.3f;
    }
    // Update is called once per frame
    void Update()
    {
        if (!dead) //if not dead
        {
            //setting axis of horizontal movement
            horizontalMovement = Input.GetAxisRaw("Horizontal") * horizontalMovementSpeed;

            if (transform.position.x > boundaryDistanceHorizontal) //if position of x is greater than the positive x of boundry 
            {
                transform.position = new Vector3(-boundaryDistanceHorizontal, transform.position.y); //Teleport to opposite side
            }
            else if (transform.position.x < -boundaryDistanceHorizontal) //if position of x ia less than the negative x of boundry 
            {
                transform.position = new Vector3(boundaryDistanceHorizontal, transform.position.y); //Teleport to opposite side
            }
        }
    }
    private void FixedUpdate()
    {
        //setting the rigidbody velocity to a vector 2 and set horrizontalMovement in the "x" Axis.
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y); 
    }
    private void LateUpdate()
    {
        if (!dead) //if bool equals not dead do this
        {
            float y = transform.position.y; //position on y axis

            if (y > highestY) //if y is higher than highest score of y 
            {
                highestY = y; //set highest y to equal y
                Score = (int)(highestY - 4.5f);
                if (Score > highScore.highScore)
                    highScoreText.text = Score.ToString();
                Vector3 pos = cameraTransform.position; // pos becomes the position of camera
                pos.y = y; //reassigning y coordinate of camera 
                cameraTransform.position = pos; //equals the character's y 
            }
            else if (highestY - y > boundaryDistanceVertical) //if character droped below the screen 
            {
                dead = true; //if character is not dead and then falls off make it dead
                menuHandler.TogglePause(dead); //run this method
                audioSource.PlayOneShot(fallSound);
                SaveHighScore();
            }
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collides with collider from tage Platform and if veolocity of y relative with the player is greator than 0
        if (collision.collider.CompareTag("Platform") && collision.relativeVelocity.y > 0)
        {
            //set rigidbody velocity of "y" to jumpforce
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            //Run the audio bounceSound
            audioSource.PlayOneShot(bounceSound);

            //Set the bool on the Animator that is called "isGrounded" to be set to true
            anim.SetBool("isGrounded", true);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision) //on exit of the collision
    {
        anim.SetBool("isGrounded", false); //set the isGrounded bool to false on the Animator
    }
    public void ResetPlayer() //Run this method
    {
        audioSource.Stop(); //Stop the audio source
        highestY = cameraTransform.position.y; //resets highest "y"
        score = 0; //resests score
        dead = false; //reset the bool dead to be false
    }

    public void LoadHighScore() //Load High Scores Method
    {
        if (File.Exists(saveFilePath)) //if there is a file
        {
            FileStream file = File.Open(saveFilePath, FileMode.Open); //open the file from the saved file path
            BinaryFormatter bf = new BinaryFormatter(); //create new BinaryFormatter
           // use the Binaryformatter and make the highscore value from "0" and "1" to a value for humans
            highScore = bf.Deserialize(file) as Score;
            file.Close(); //close the file
        }
        else //if there is no file 
        {
            highScore = new Score(); //highscore equals new class called Score
        }
        highScoreText.text = highScore.highScore.ToString(); //change highscore tect
    }
    public void SaveHighScore() //Save High Scores Method
    {
        if (score > highScore.highScore) //if score is higher than highscore
        {
            highScore.highScore = score; //make highscore to equal score
            highScoreText.text = highScore.highScore.ToString(); //change the text 
            FileStream file = File.Open(saveFilePath, FileMode.OpenOrCreate); //Either open or create a file using file path
            BinaryFormatter bf = new BinaryFormatter(); //create a new BinaryFormatter
            bf.Serialize(file, highScore); //Make the score into Binary "0" and "1" USING BinaryFormatter
            file.Close(); //close the file
        }
    }
}
