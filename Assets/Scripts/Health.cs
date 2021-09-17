using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Health : MonoBehaviour, IPunObservable
{
    [SerializeField] protected float initialHealth;

    protected float health;

    private void Awake()
    {
        health = initialHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetRoundedHealth()
    {
        return Mathf.RoundToInt(health);
    }

    public virtual void ChangeHealth(float change)
    {
        health += change;

        if (health > 0) return;

        OnDeath();
    }

    [PunRPC]
    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            return;
        }

        object received = stream.ReceiveNext();
        health = (float)received;
    }
}