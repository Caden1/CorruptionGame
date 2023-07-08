using UnityEngine;
using UnityEngine.UIElements;

public class SwapUI
{
	private VisualElement silhouetteEl;

	public SwapUI(UIDocument gemSwapUIDoc) {
		if (gemSwapUIDoc != null) {

			silhouetteEl = gemSwapUIDoc.rootVisualElement.Q("PlayerSilhouette") as VisualElement;
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
}
