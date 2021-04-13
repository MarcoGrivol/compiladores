using UnityEngine;

public class Shell : WarEntity {
    // controla o projétil

    Vector3 launchPoint, targetPoint, launchVelocity;

    float age, blastRadius, damage;

    public void Initialize(
        Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity,
        float blastRadius, float damage
    ) {
        this.launchPoint = launchPoint;
        this.targetPoint = targetPoint;
        this.launchVelocity = launchVelocity;
        this.blastRadius = blastRadius;
		this.damage = damage;
    }

    public override bool GameUpdate() {
        // controla o movimento
        age += Time.deltaTime;
        Vector3 p = launchPoint + launchVelocity * age;
        p.y -= 0.5f * 9.81f * age * age;

        if (p.y <= 0f) {
            // atingiu o solo -> detona
            try {
                Game.SpawnExplosion().Initialize(targetPoint, blastRadius, damage);
            }
            catch {
                GameCompiler.SpawnExplosion().Initialize(targetPoint, blastRadius, damage);
            }
            OriginFactory.Reclaim(this);
			return false;
		}

        transform.localPosition = p;

        Vector3 d = launchVelocity;
		d.y -= 9.81f * age;
		transform.localRotation = Quaternion.LookRotation(d);
        try {
            Game.SpawnExplosion().Initialize(p, 0.1f);
        }
        catch {
            GameCompiler.SpawnExplosion().Initialize(p, 0.1f);
        }
        return true;
    }
}