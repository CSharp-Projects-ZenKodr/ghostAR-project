using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour { 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("gameplay");
        }
    }
}