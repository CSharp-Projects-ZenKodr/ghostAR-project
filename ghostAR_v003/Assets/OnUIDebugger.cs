

using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class OnUIDebugger : MonoBehaviour {

    public Text debugText;

    private void Update() {
        ListDebugData();
    }

    void ListDebugData() {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine();

        debugText.text = sb.ToString();
    }
}