using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarUI
{
	private VisualElement healthBarEl;

	public HealthBarUI(UIDocument healthBarUIDoc) {
        if (healthBarUIDoc != null) {
			healthBarEl = healthBarUIDoc.rootVisualElement.Q("HealthBar") as VisualElement;
		}
	}

	public void DecreaseHealthBarSize(float percentDecimal) {
		float percentWholeNum = percentDecimal * 100f;
		if (healthBarEl != null) {
			healthBarEl.style.width = Length.Percent(percentWholeNum);
		}
	}
}
