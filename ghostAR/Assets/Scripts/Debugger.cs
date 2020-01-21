using UnityEngine;

public class Debugger : MonoBehaviour { 

    public int killPts;
    public int dmgVal;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.IncreaseScore(killPts);
            GameManager.Instance.playerHealth.DamageObject(dmgVal);
        }
    }
}