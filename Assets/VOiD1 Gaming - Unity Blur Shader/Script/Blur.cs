using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
class Blur : MonoBehaviour {

	[Tooltip("The Blur Shader attahed with this Asset.")]
	public Shader blurShader;

	[Tooltip("The Blur Material created using the Blur Script.")]
	public Material blurMaterial;	

	[Tooltip("Strength of the Blur. Governs the Area of Pixels to Blend.")]
	[Range(0f,10f)] 	
	public float radius = 3.0f;

	[Tooltip("Iterations of Blur Calculation.")]
	[Range(1,6)] 
	public int qualityIterations = 2;

	[Tooltip("Improves quality and reduces sharp edges (Downsamples the Image).")]
	[Range(0,3)] 
	public int filter = 1;

	void OnRenderImage (RenderTexture sourcert, RenderTexture destinationrt) {	

		if(blurShader == null){
			return;
		}
		
		float widthModification = 1.0f / (1.0f * (1<<filter));
		
		blurMaterial.SetVector("_Param", new Vector4 (radius * widthModification, -radius * widthModification, 0f, 0f));
		sourcert.filterMode = FilterMode.Bilinear;
		
		int rendertextureWidth = sourcert.width >> filter;
		int rendertextureHeight = sourcert.height >> filter;
		
		RenderTexture rendertexture = RenderTexture.GetTemporary (rendertextureWidth, rendertextureHeight, 0, sourcert.format);
		
		rendertexture.filterMode = FilterMode.Bilinear;
		Graphics.Blit (sourcert, rendertexture, blurMaterial, 0);
				
		for(int i = 0; i < qualityIterations; i++) {
			float iterationOffset = (i*1.0f);
			blurMaterial.SetVector ("_Param", new Vector4 (radius * widthModification + iterationOffset, -radius * widthModification - iterationOffset, 0.0f, 0.0f));
			RenderTexture rendertexturetemp = RenderTexture.GetTemporary (rendertextureWidth, rendertextureHeight, 0, sourcert.format);
			rendertexturetemp.filterMode = FilterMode.Bilinear;
			Graphics.Blit (rendertexture, rendertexturetemp, blurMaterial, 1);
			RenderTexture.ReleaseTemporary (rendertexture);
			rendertexture = rendertexturetemp;		
			rendertexturetemp = RenderTexture.GetTemporary (rendertextureWidth, rendertextureHeight, 0, sourcert.format);
			rendertexturetemp.filterMode = FilterMode.Bilinear;
			Graphics.Blit (rendertexture, rendertexturetemp, blurMaterial, 2);
			RenderTexture.ReleaseTemporary (rendertexture);
			rendertexture = rendertexturetemp;
		}
		Graphics.Blit (rendertexture, destinationrt);
		RenderTexture.ReleaseTemporary (rendertexture);
	}	
}
