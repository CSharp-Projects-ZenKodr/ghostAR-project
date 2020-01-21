using System;

using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public GameObject[] tutorialScreens;

    private int currentScreen;
    
	void Start () {
        tutorialScreens = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            tutorialScreens[i] = transform.GetChild(i).gameObject;
        }
	}

	void Update () {
        ScreenManager();
	}

    void ScreenManager() {
       Array.cont
    }
}