using UnityEngine;
using System.Collections;

public class MouesPosition : MonoBehaviour {

    Vector3 MousePos;
    public static Vector3 camScreen;
    public static Vector3 m_ScreenMosPos;
    public static float distance = 3913;
   
    void Awake()
    {
        camScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.farClipPlane - 1));
    }
	
	// Update is called once per frame
	void Update ()
    {
        camScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.farClipPlane - 1));
        MousePos = Input.mousePosition;
        MousePos.z = distance;
        m_ScreenMosPos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, Camera.main.farClipPlane));

    }
}
