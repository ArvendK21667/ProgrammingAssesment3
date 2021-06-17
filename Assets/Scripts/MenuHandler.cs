using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    private bool paused;
    public bool Paused 
    {
        get { return paused; } //gets the return value of the bool of paused
        set 
        {
            paused = value; //returns the value of pause equal value
            Time.timeScale = paused ? 0 : 1; //sets the time scale to equal 0 or 1 based on "pause" equalling true or false
            menuPanel.SetActive(paused); //sets the menu panel being active based on the value of "paused"
        }
    }
    private bool canPause;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Player player;
    [SerializeField] private PlatformSpawner platformSpawner;
    [SerializeField] private UnityEngine.Audio.AudioMixer audioMixer;
    [SerializeField] private UnityEngine.UI.Slider audioSlider;

    private const string audioString = "MasterVolume";
    // Start is called before the first frame update
    void Start()
    {
        LoadAudioSettings(); //Do LoadAudioSettings Method
        StartGame(); //Do StartGame Method
        Paused = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //if press esc
        {
            if (canPause) // a bool if statement 
                TogglePause(); //do this Method
        }
    }
    public void LoadAudioSettings() //Loading Audio Settings 
    {
        float volume = PlayerPrefs.GetFloat(audioString, 0);
        audioSlider.value = volume;
        audioMixer.SetFloat(audioString, volume);
    }
    public void AudioControl(float volume) //Audio Control Method
    {
        audioMixer.SetFloat(audioString, volume); //uses adiomixer to adust volume
        PlayerPrefs.SetFloat(audioString, volume); //Saves volume to playerprefs
    }
    public void StartGame() //Starts game with onclick
    {
        canPause = true;
        Paused = false;
        player.transform.position = new Vector3(0, 0.5f, 0); //Sets the character back to startpoint
        Camera.main.transform.position = new Vector3(0, 4.5f, -10); //Sets the camera back to startpoint 
        player.ResetPlayer(); //does this Method
        platformSpawner.NewGame();
    }
    public void TogglePause(bool dead = false) // making a bool named dead false
    {
        if (dead)
            canPause = !dead; //currently is true
        if (canPause || dead)
            Paused = !Paused; //currently euqals the opposite of what the value of Paused is set to
    }
    public void ExitGame() //Quits game
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
