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
        Debug.Log($"{comp.gameObject.activeSelf} {comp.gameObject.activeInHierarchy} {comp.enabled} {comp.isActiveAndEnabled}");
        comp.Play();
    }

    public void LoadingScene(int numberScene)
    {
        SceneManager.LoadScene(numberScene);
    }
}
