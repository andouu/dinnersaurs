Shader "VOiD1Gaming/UnityBlurShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Bloom ("Bloom (RGB)", 2D) = "black" {}
	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _Bloom;
				
		uniform half4 _MainTex_TexelSize;
		uniform half4 _Param;

		struct v2f_tap
		{
			float4 pos : SV_POSITION;
			half2 uv20 : TEXCOORD0;
			half2 uv21 : TEXCOORD1;
			half2 uv22 : TEXCOORD2;
			half2 uv23 : TEXCOORD3;
		};	

		static const half curve[7] = { 0.0205, 0.0855, 0.232, 0.324, 0.232, 0.0855, 0.0205 };

		static const half4 curve4[7] = { half4(0.0205,0.0205,0.0205,0), half4(0.0855,0.0855,0.0855,0), half4(0.232,0.232,0.232,0),
			half4(0.324,0.324,0.324,1), half4(0.232,0.232,0.232,0), half4(0.0855,0.0855,0.0855,0), half4(0.0205,0.0205,0.0205,0) };

		struct v2f_withBlurCoords8
		{
			float4 pos : SV_POSITION;
			half4 uv : TEXCOORD0;
			half2 offs : TEXCOORD1;
		};

		half4 fragBlur8(v2f_withBlurCoords8 i) : COLOR
		{
			half2 uv = i.uv.xy;
			half2 netFilterWidth = i.offs;
			half2 coords = uv - netFilterWidth * 3.0;

			half4 color = 0;
			for (int l = 0; l < 7; l++)
			{
				half4 tap = tex2D(_MainTex, coords);
				color += tap * curve4[l];
				coords += netFilterWidth;
			}
			return color;
		}

		v2f_tap vert4Tap ( appdata_img v )
		{
			v2f_tap o;

			o.pos = UnityObjectToClipPos (v.vertex);
        	o.uv20 = v.texcoord + _MainTex_TexelSize.xy;				
			o.uv21 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h,-0.5h);	
			o.uv22 = v.texcoord + _MainTex_TexelSize.xy * half2(0.5h,-0.5h);		
			o.uv23 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h,0.5h);		

			return o; 
		}					
		
		fixed4 fragDownsample ( v2f_tap i ) : COLOR
		{				
			fixed4 color = tex2D (_MainTex, i.uv20);
			color += tex2D (_MainTex, i.uv21);
			color += tex2D (_MainTex, i.uv22);
			color += tex2D (_MainTex, i.uv23);
			return color / 4;
		}
	

		v2f_withBlurCoords8 vertBlurHorizontal (appdata_img v)
		{
			v2f_withBlurCoords8 o;
			o.pos = UnityObjectToClipPos (v.vertex);
			
			o.uv = half4(v.texcoord.xy,1,1);
			o.offs = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Param.x;

			return o; 
		}
		
		v2f_withBlurCoords8 vertBlurVertical (appdata_img v)
		{
			v2f_withBlurCoords8 o;
			o.pos = UnityObjectToClipPos (v.vertex);
			
			o.uv = half4(v.texcoord.xy,1,1);
			o.offs = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Param.x;
			 
			return o; 
		}	

	ENDCG
	
	SubShader {
	  ZTest Off Cull Off ZWrite Off Blend Off
	  Fog { Mode off }  

	Pass { 
	
		CGPROGRAM
		
		#pragma vertex vert4Tap
		#pragma fragment fragDownsample
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG
		 
		}
		  
		Pass {

		ZTest Always
		Cull Off
		
		CGPROGRAM 
		
		#pragma vertex vertBlurVertical
		#pragma fragment fragBlur8
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG 
		}	
		
	Pass {		
		ZTest Always
		Cull Off
				
		CGPROGRAM
		
		#pragma vertex vertBlurHorizontal
		#pragma fragment fragBlur8
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG
		}	
	}	

	FallBack Off
}
