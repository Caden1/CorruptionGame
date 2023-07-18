using UnityEngine;

public class SwapUISprites : MonoBehaviour
{
	// silhouettes
	public Sprite purOnlyFeetSilhouette;
	public Sprite purOnlyHandsSilhouette;
	public Sprite purFeetCorHandsSilhouette;
	public Sprite purHandsCorFeetSilhouette;

	// feet: purity, hands: corruption
	public Sprite purFeetAllMods1Silhouette; // left hand: air, right hand: fire, right foot: water, left foot: earth
	public Sprite purFeetAllMods2Silhouette; // left hand: earth, right hand: air, right foot: fire, left foot: water
	public Sprite purFeetAllMods3Silhouette; // left hand: water, right hand: earth, right foot: air, left foot: fire
	public Sprite purFeetAllMods4Silhouette; // left hand: fire, right hand: water, right foot: earth, left foot: air

	// feet: corruption, hands: purity
	public Sprite purHandsAllMods1Silhouette; // left hand: air, right hand: fire, right foot: water, left foot: earth
	public Sprite purHandsAllMods2Silhouette; // left hand: earth, right hand: air, right foot: fire, left foot: water
	public Sprite purHandsAllMods3Silhouette; // left hand: water, right hand: earth, right foot: air, left foot: fire
	public Sprite purHandsAllMods4Silhouette; // left hand: fire, right hand: water, right foot: earth, left foot: air

	// no gems
	public Sprite noGemRightHandIcon;
	public Sprite noGemLeftHandIcon;
	public Sprite noGemRightFootIcon;
	public Sprite noGemLeftFootIcon;

	// purity only
	public Sprite purityOnlyRightHandIcon;
	public Sprite purityOnlyLeftHandIcon;
	public Sprite purityOnlyRightFootIcon;
	public Sprite purityOnlyLeftFootIcon;

	// corruption only
	public Sprite corruptionOnlyRightHandIcon;
	public Sprite corruptionOnlyLeftHandIcon;
	public Sprite corruptionOnlyRightFootIcon;
	public Sprite corruptionOnlyLeftFootIcon;

	// purity air
	public Sprite purityAirRightHandIcon;
	public Sprite purityAirLeftHandIcon;
	public Sprite purityAirRightFootIcon;
	public Sprite purityAirLeftFootIcon;

	// corruption air
	public Sprite corruptionAirRightHandIcon;
	public Sprite corruptionAirLeftHandIcon;
	public Sprite corruptionAirRightFootIcon;
	public Sprite corruptionAirLeftFootIcon;
}
