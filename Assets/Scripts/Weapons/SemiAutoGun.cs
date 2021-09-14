namespace Weapons
{
    public class SemiAutoGun : Gun
    {
        public override void ToggleFire(bool isFiring)
        {
            if (fireCooldown > 0 || bulletsInMagazine < 1) return;

            if (reloadCoroutine != null)
            {
                StopCoroutine(reloadCoroutine);
            }

            Fire();
        }
    }
}