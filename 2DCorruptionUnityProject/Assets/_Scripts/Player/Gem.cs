using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gem : MonoBehaviour
{
	public string gemName;
}

public class BaseGem : Gem
{
	public float moveSpeed;
	public float jumpForce;
	public float dashForce;
}

public class ModifierGem : Gem
{
	
}
