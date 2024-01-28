using UnityEngine;

public class CircusGunRigidbody : CircusGun
{
    public GameObject projectilePrefab;
    public float muzzleVelocity = 120;

    protected override void DoFire()
    {
        PlayGunSFX(gunSFX.RUN);
        var projectile = Instantiate(projectilePrefab);
        projectile.transform.SetPositionAndRotation(muzzleTransform.position, muzzleTransform.rotation);
        var rigidbody = projectile.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.AddForce(muzzleTransform.forward * muzzleVelocity, ForceMode.VelocityChange);
            rigidbody.AddTorque(new Vector3(Random.value, Random.value, Random.value) * 40, ForceMode.VelocityChange);
        }
    }
}