using Interact;

public class PlayerHealth : HealthController
{
    private PlayerInteract _playerInteract;
    private void Start()
    {
        _playerInteract = transform.GetComponent<PlayerInteract>();
    }

    public override void ChangeHealth(int change)
    {
        if (change < 0) _playerInteract.CancelHoldInteraction();
        base.ChangeHealth(change);
    }

}
