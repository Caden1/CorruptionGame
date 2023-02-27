using UnityEngine;
using UnityEngine.UIElements;

public class SwapUI
{
	private VisualElement glovesEl;
	private VisualElement bootsEl;
	private VisualElement rightGloveEl;
	private VisualElement leftGloveEl;
	private VisualElement rightBootEl;
	private VisualElement leftBootEl;

	public SwapUI(UIDocument gemSwapUIDoc) {
		if (gemSwapUIDoc != null) {
			glovesEl = gemSwapUIDoc.rootVisualElement.Q("GlovesContainer") as VisualElement;
			if (glovesEl != null) {
				rightGloveEl = glovesEl.Q("RightGlove") as VisualElement;
				leftGloveEl = glovesEl.Q("LeftGlove") as VisualElement;
			}

			bootsEl = gemSwapUIDoc.rootVisualElement.Q("BootsContainer") as VisualElement;
			if (bootsEl != null) {
				rightBootEl = bootsEl.Q("RightBoot") as VisualElement;
				leftBootEl = bootsEl.Q("LeftBoot") as VisualElement;
			}
		}
	}

	public void RemoveRightGloveGem() {
		if (rightGloveEl != null) {
			rightGloveEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void RemoveLeftGloveGem() {
		if (leftGloveEl != null) {
			leftGloveEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void RemoveRightBootGem() {
		if (rightBootEl != null) {
			rightBootEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void RemoveLeftBootGem() {
		if (leftBootEl != null) {
			leftBootEl.style.backgroundImage = new StyleBackground();
		}
	}

	public void SetRightGloveGem(Sprite gem) {
		if (rightGloveEl != null) {
			rightGloveEl.style.backgroundImage = new StyleBackground(gem);
		}
	}

	public void SetLeftGloveGem(Sprite gem) {
		if (leftGloveEl != null) {
			leftGloveEl.style.backgroundImage = new StyleBackground(gem);
		}
	}

	public void SetRightBootGem(Sprite gem) {
		if (rightBootEl != null) {
			rightBootEl.style.backgroundImage = new StyleBackground(gem);
		}
	}

	public void SetLeftBootGem(Sprite gem) {
		if (leftBootEl != null) {
			leftBootEl.style.backgroundImage = new StyleBackground(gem);
		}
	}
}
