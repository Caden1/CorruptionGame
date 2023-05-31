using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skills
{
	public static bool lockMovement { get; protected set; }
	public static bool isInvulnerable { get; protected set; }
	public static bool hasForcedMovement { get; protected set; }
	public static Vector2 forcedMovementVector { get; protected set; }
}
