using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{

    [SerializeField] private GameObject menuPanel;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() //Starts game with onclick
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame() //Quits game
    {
        Application.Quit();
    }


}
