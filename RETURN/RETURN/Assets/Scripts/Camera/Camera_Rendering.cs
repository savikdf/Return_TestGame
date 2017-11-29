using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class Camera_Rendering : UnityStandardAssets.ImageEffects.ImageEffectBase
{
    public Material mat;
    public int horizontalResolution = 320;
    public int verticalResolution = 240;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //To draw the shader at the full resolution, use :
        Graphics.Blit(source, destination, mat);
    }
}
