using UnityEngine;

public class DoNotDestroyBGMusic : MonoBehaviour
{
    private void Awake()
    {
        var bgMusic = GameObject.FindGameObjectsWithTag("BGMusic");
        if (bgMusic.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
