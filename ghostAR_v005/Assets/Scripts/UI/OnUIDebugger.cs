using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class OnUIDebugger : MonoBehaviour {
    /// <summary>
    /// The Text that will display the debugging data.
    /// </summary>
    [Tooltip("The Text that will display the debugging data.")]
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