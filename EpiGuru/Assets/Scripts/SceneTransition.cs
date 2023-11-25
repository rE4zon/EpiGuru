using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Button myButton;
    [SerializeField] private string targetSceneName;

    void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonPress);
        }
        else
        {
            Debug.LogError("No button in inspector");
        }
    }

    public void OnButtonPress()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
