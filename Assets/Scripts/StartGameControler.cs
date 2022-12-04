using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameControler : MonoBehaviour
{
    public void RouteScreen(string gameMode)
    {
        AddButton.gameMode = gameMode;
        SceneManager.LoadScene("InGame");
    }
}
