using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class OnUIDebugger : MonoBehaviour {

    public FriendlyGhost friendly;
    public Text debugText;

    private void Update() {
        ListDebugData();
    }

    void ListDebugData() {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Fade Status: " + friendly.currentFadeStatus);
        sb.AppendLine("Moving: " + friendly._Moving.ToString());
        sb.AppendLine("Current Health: " + friendly.ghostHealth.CurrentHealth.ToString());
        if (friendly.ghostHealth.CurrentHealth == friendly.ghostHealth.maxHealth) {
            sb.AppendLine("Current Health is Maximum Health!");
        } else {
            sb.AppendLine("Current Health is less than Maximum Health!");
        }
        if (friendly.giveHealth) {

            sb.AppendLine("Giving Health");
        } else {
            sb.AppendLine("Giving Advise");
        }
        sb.AppendLine("Hostile Distance: " + friendly.distanceFromHostile);
        sb.AppendLine("Current Speed: " + friendly._CurrentSpeed.ToString());

        debugText.text = sb.ToString();
    }
}
