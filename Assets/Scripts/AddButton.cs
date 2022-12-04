using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButton : MonoBehaviour
{
    public static string gameMode = "easy";
    
    public int numBtn;
    public Transform panel;
    public GameObject pikachuBtn;
    
    private static int easy = 16;
    private static int medium = 36;
    private static int hard = 64;

    void Awake()
    {
        GameObject btn;

        if (gameMode == "medium")
        {
            numBtn = medium;
        } else if (gameMode == "hard")
        {
            numBtn = hard;
        }
        else
        {
            numBtn = easy;
        }
        for (int i=0; i<numBtn; i++)
        {
            btn = Instantiate(pikachuBtn);
            btn.name = i.ToString();
            btn.transform.SetParent(panel);
        }
    }
}
