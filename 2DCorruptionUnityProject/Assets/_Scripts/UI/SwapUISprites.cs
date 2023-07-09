using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapUISprites : MonoBehaviour
{
	public Sprite purityFeetSilhouette;
	public Sprite purityHandsSilhouette;

	public Sprite noGemJumpIcon;

	public Sprite purityPushIcon;
	public Sprite purityPullIcon;
	public Sprite purityJumpIcon;
	public Sprite purityDashIcon;

	public Sprite GetPurityFeetSilhouette() {
		return purityFeetSilhouette;
	}

	public Sprite GetPurityHandsSilhouette() {
		return purityHandsSilhouette;
	}

	public Sprite GetNoGemJumpIcon() {
		return noGemJumpIcon;
	}

	public Sprite GetPurityPushIcon() {
		return purityPushIcon;
	}

	public Sprite GetPurityPullIcon() {
		return purityPullIcon;
	}

	public Sprite GetPurityJumpIcon() {
		return purityJumpIcon;
	}

	public Sprite GetPurityDashIcon() {
		return purityDashIcon;
	}
}
