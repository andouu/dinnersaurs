using UnityEngine;

[ExecuteInEditMode]
public class CameraColorFix : MonoBehaviour
{
    public Material material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        print("done");
        Graphics.Blit(source, destination, material);
        return;
    }
}