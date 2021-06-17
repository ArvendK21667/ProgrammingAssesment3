using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Score
{
    public int highScore = 100;
}


[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] float horizontalMovementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] TMP_Text currentScoreText, highScoreText;
    private float horizontalMovement;
    private Rigidbody2D rb;
    private float boundaryDistanceVertical;
    private float boundaryDistanceHorizontal;
    private float highestY;
    private float score;
    private Score highScore;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //making the distance boundry to equal the orthograpghic size proportional to the width/height of screen
        boundaryDistanceHorizontal = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height;
        //making the distance boundry to equal the orthograpghic 
        boundaryDistanceVertical = Camera.main.orthographicSize + 0.3f;
    }

    // Update is called once per frame
    void Update()
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
    private void FixedUpdate()
    {
        //setting the rigidbody velocity to a vector 2 and set horrizontalMovement in the "x" Axis.
        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y); 
    }
    private void LateUpdate()
    {
        float y = transform.position.y;
        
        if (y > highestY) //if y is higher than highest score of y 
        {
            highestY = y; //set highest y to equal y
            Vector3 pos = Camera.main.transform.position; // pos becomes the position of camera
            pos.y = y; //reassigning y coordinate of camera 
            Camera.main.transform.position = pos; //equals the character's y 
        }
        else if (highestY - y > boundaryDistanceVertical) //if character droped below the screen 
        {
            menuPanel.SetActive(true); //opens Menu Panel when dead
            Time.timeScale = 0; //pause time
            //Debug.Log("Dead"); 
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collides with collider from tage Platform and if veolocity of y relative with the player is greator than 0
        if (collision.collider.CompareTag("Platform") && collision.relativeVelocity.y > 0)
        {
            //set rigidbody velocity of "y" to jumpforce
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
