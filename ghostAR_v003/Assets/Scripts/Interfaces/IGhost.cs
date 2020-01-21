using UnityEngine;

public interface IGhost {
    ComboSystem comSystem { get; set; }
    Color fullColor { get; set; }
    GameManager gameMan { get; set; }
    GameObject playerObject { get; set; }
    Health ghostHealth { get; set; }
    Health playerHealth { get; set; }
    ObjectShake ghostShake { get; set; }
    MeshRenderer ghostRenderer { get; set; }

    void CheckingFadeStatus();
    void FadeModelIn();
    void FadeModelOut();
    void MovingAroundThePlayer();
    void PickDirection();
    void RestrictingValues();
}