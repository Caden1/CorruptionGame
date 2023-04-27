using UnityEngine;

public abstract class EnemyResistancesAndWeaknesses
{
	public float corDamageResist { get; protected set; }
	public float corAirDamageResist { get; protected set; }
	public float corFireDamageResist { get; protected set; }
	public float corWaterDamageResist { get; protected set; }
	public float corEarthDamageResist { get; protected set; }

	public float corDamageWeakness { get; protected set; }
	public float corAirDamageWeakness { get; protected set; }
	public float corFireDamageWeakness { get; protected set; }
	public float corWaterDamageWeakness { get; protected set; }
	public float corEarthDamageWeakness { get; protected set; }

	public float uppercutKnockupVelocityResist { get; protected set; }
	public float kickDashKnockbackVelocityResist { get; protected set; }
	public float punchKnockbackVelocityResist { get; protected set; }
	public float pushbackVelocityResist { get; protected set; }
	public float pullSpeedResist { get; protected set; }
}
