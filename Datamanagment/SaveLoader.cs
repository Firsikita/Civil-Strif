using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoader : MonoBehaviour
{
   public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
        Time.timeScale = 1f;  
    }
}
