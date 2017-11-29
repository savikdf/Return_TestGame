using UnityEngine;
using System.Collections;

public enum AdjacentType{
	Left,
	Right,
	Up,
	Down
};

public class Adjacent : MonoBehaviour {
	public Transform adjObj;
	public AdjacentType adjType;

}
