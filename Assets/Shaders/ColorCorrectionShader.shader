Shader "Unlit/ColorCorrectionShader"
{
    Properties
    {
        _Tex ("InputTex", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Lighting Off
        Blend One Zero
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"

            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4 _Color;
            sampler2D _Tex;

            fixed4 frag (v2f_init_customrendertexture IN) : COLOR
            {
                // sample the texture
                float4 col = tex2D(_Tex, IN.texcoord.xy);
                col.rgb = GammaToLinearSpace(col.rgb);
                return col;
                //return _Color * tex2D(_Tex, IN.texcoord.xy);
            }
            ENDCG
        }
    }
}
