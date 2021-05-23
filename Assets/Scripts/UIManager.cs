using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public void Check()
    {
        var comp = GetComponent<AudioSource>();
        comp.Play();
    }

    public void LoadingScene(int numberScene)
    {
        SceneManager.LoadScene(numberScene);
    }
}
