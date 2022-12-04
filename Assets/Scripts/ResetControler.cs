using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetControler : MonoBehaviour
{
    public void RouteLevel()
    {
        SceneManager.LoadScene("StartGame");
    }
}
