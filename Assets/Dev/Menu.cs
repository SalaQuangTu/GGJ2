using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public LevelSelector lvlSelector;
    public Text textStart;

    public bool onLoad = false;

    private void Start()
    {
        onLoad = false;
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Fire1") == 1 && !onLoad)
        {
            lvlSelector.LoadNextLevel();
            onLoad = true;
        }
    }
}