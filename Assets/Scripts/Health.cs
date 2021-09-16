using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth;

    private float _health;

    private void Awake()
    {
        _health = initialHealth;
    }

    public float GetHealth()
    {
        return _health;
    }

    public int GetRoundedHealth()
    {
        return Mathf.RoundToInt(_health);
    }

    public void ChangeHealth(float change)
    {
        _health += change;

        if (_health > 0) return;

        OnDeath();
    }

    private void OnDeath()
    {
        Destroy(gameObject);
        gameObject.SendMessage("DropSingleItem", SendMessageOptions.RequireReceiver);
    }
}