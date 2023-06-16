using UnityEngine;
using UnityEngine.UIElements;

public class TutorialUI
{
	//private Label jumpLabel;
	private VisualElement jumpLabelEl;

	public TutorialUI(UIDocument tutorialUIDoc) {
		if (tutorialUIDoc != null) {
			jumpLabelEl = tutorialUIDoc.rootVisualElement.Q("JumpLabel") as VisualElement;
		}
	}

	public void DisplayJumpLabel() {

	}

	public void RemoveJumpLabel() {

	}
}
