using UnityEngine;
using UnityEngine.UIElements;

public class SwapUI
{
	private VisualElement silhouetteEl;
	private VisualElement rightHandIconEl;
	private VisualElement leftHandIconEl;
	private VisualElement rightFootIconEl;
	private VisualElement lefFootIconEl;

	public SwapUI(UIDocument gemSwapUIDoc) {
		if (gemSwapUIDoc != null) {
			silhouetteEl = gemSwapUIDoc.rootVisualElement.Q("PlayerSilhouette");
			rightHandIconEl = gemSwapUIDoc.rootVisualElement.Q("RightHandIcon");
			leftHandIconEl = gemSwapUIDoc.rootVisualElement.Q("LeftHandIcon");
			rightFootIconEl = gemSwapUIDoc.rootVisualElement.Q("RightFootIcon");
			lefFootIconEl = gemSwapUIDoc.rootVisualElement.Q("LeftFootIcon");
		}
	}

	public void RemoveSilhouette() {
		if (silhouetteEl != null) {
			silhouetteEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void SetSilhouette(Sprite silhouette) {
		if (silhouetteEl != null) {
			silhouetteEl.style.backgroundImage = new StyleBackground(silhouette);
		}
	}

	public void RemoveRightHandIcon() {
		if (rightHandIconEl != null) {
			rightHandIconEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void SetRightHandIcon(Sprite icon) {
		if (rightHandIconEl != null) {
			rightHandIconEl.style.backgroundImage = new StyleBackground(icon);
		}
	}

	public void RemoveLeftHandIcon() {
		if (leftHandIconEl != null) {
			leftHandIconEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void SetLeftHandIcon(Sprite icon) {
		if (leftHandIconEl != null) {
			leftHandIconEl.style.backgroundImage = new StyleBackground(icon);
		}
	}

	public void RemoveRightFootIcon() {
		if (rightFootIconEl != null) {
			rightFootIconEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void SetRightFootIcon(Sprite icon) {
		if (rightFootIconEl != null) {
			rightFootIconEl.style.backgroundImage = new StyleBackground(icon);
		}
	}

	public void RemoveLeftFootIcon() {
		if (lefFootIconEl != null) {
			lefFootIconEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void SetLeftFootIcon(Sprite icon) {
		if (lefFootIconEl != null) {
			lefFootIconEl.style.backgroundImage = new StyleBackground(icon);
		}
	}
}
