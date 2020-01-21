using UnityEngine;
using Boo;

public class Health : MonoBehaviour {

    #region Variables
    public enum DamagableType { PLAYER, HOSTILE, FRIENDLY }
    
    public DamagableType componentTag;

    /// <summary>
    /// The object's maximum amount of health it can have.
    /// </summary>
    [Tooltip("The object's maximum amount of health it can have.")]
    public int maxHealth;

    /// <summary>
    /// The current amount of health the object has.
    /// </summary>
    public int CurrentHealth { get; set; }

    /// <summary>
    /// Return true when CurrentHealth value is less than or equal to zero.
    /// </summary>
    private bool dead = false;

    /// <summary>
    /// Return true when Object has invincibility.
    /// </summary>
    [HideInInspector]
    public bool invincible = false;

    /// <summary>
    /// Returns true if Object is dead.
    /// </summary>
    public bool IsDead { get { return dead; } }
    #endregion

    private void Start()
    {
        CurrentHealth = maxHealth;
        if (componentTag == DamagableType.PLAYER) {
            invincible = false;
        }
    }

    #region Public Methods
    /// <summary>
    /// Damages the object's Current Health.  Also checks if Object has died.
    /// </summary>
    /// <param name="damageValue">
    /// (int) Value to damage object.
    /// </param>
    public void DamageObject (int damageValue)
    {
        if (!invincible) {
            if (CurrentHealth > 0) {
                CurrentHealth -= damageValue;
                DeathCheck();
                PrintCurrentHealth();
            }
        } else {
            Debug.Log(this.name + " is invincible.  It cannot be damaged at this time.");
        }
    }

    /// <summary>
    /// Heal's the object's Current Health if it does not already have full health.
    /// </summary>
    /// <param name="healValue">
    /// (int) Value to heal object.
    /// </param>
    public void HealObject (int healValue)
    {
        if (CurrentHealth < maxHealth) {
            CurrentHealth += healValue;
        } else if (CurrentHealth >= maxHealth)
        {
            Debug.Log(name + "'s health is full. Cannot heal.");
        }
        PrintCurrentHealth();
    }

    /// <summary>
    /// Prints the object's current health to the console.
    /// </summary>
    public void PrintCurrentHealth () { Debug.Log(name + "'s current health is at " + CurrentHealth); }
    #endregion

    void DeathCheck()
    {
        if (CurrentHealth <= 0)
        {
            dead = true;
            invincible = true;
            Debug.LogError(name + " has died.");
        }
    }
}