using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScore : MonoBehaviour
{
    private Text leveleText;

    private void Awake()
    {
        leveleText = GetComponent<Text>();
    }
    public void ChangeText(int level)
    {
        leveleText.text = (level + 1).ToString();
    }
}
