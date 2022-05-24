using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] float delayInScene = 2f;

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadTheEnd()
    {
        StartCoroutine(WaitAndLoad()); 
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInScene);
        SceneManager.LoadScene("TheEnd");
    }
}
