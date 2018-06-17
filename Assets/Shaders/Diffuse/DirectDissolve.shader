Shader "Custom/DirectDissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_StartingVector("Starting point of the dissolve", Vector) = (0, 0, 0, 0)
		_DissolveSize("Size of the dissolve", float) = 0.0
		_DissolveTexture("Texture of the dissolve", 2D) = "white" {}
		_SliceAmount("Slice amount", float) = 0.0

		_ColorEmission("Color emission", Color) = (1,1,1,1)

		_StartingY("Starting point", float) = 0.0

		_Speed("Dissolve speed", float) = 0.05

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DissolveTexture;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float2 uv_DissolveTexture;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float3 _StartingVector;
		float _DissolveSize;
		float _SliceAmount;

		float _StartingY;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float _DissolveTextureCalc = (tex2D(_DissolveTexture, IN.uv_DissolveTexture) * _DissolveSize) + _SliceAmount;

			float transition = _StartingVector.y - IN.worldPos.y;
			float transitionX = _StartingVector.x - IN.worldPos.x;
			
			float trans1 = _StartingY + (transition + _DissolveTextureCalc);
			float trans2 = _StartingY + (transitionX + _DissolveTextureCalc);
			float trans3 = _StartingY - (transition - _DissolveTextureCalc);
			float trans4 = _StartingY - (transitionX - _DissolveTextureCalc);

			float trans5 = min(trans1, trans3) * -1;
			float trans6 = min(trans2, trans4) * -1;

			clip(min(trans5, trans6));


			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
