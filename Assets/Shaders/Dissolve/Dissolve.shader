Shader "Daniel/Dissolve"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_DissolveY("Current Y of the Dissolve effect", Float) = 0
		_DissolveSize("Size of the effect", Float) = 2
		_StartingY("Starting point of the effect", Float) = -10
		_ObjectColor("The color of the object",Color) = (0, 0, 1, 1) // R, G, B, A
	}
	SubShader
	{
		Tags{ 
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase" 
		}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#pragma multi_compile_fwdbase
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float intensity : TEXCOORD4;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _DissolveTexture;
			float _DissolveY;
			float _DissolveSize;
			float _StartingY;
			half4 _ObjectColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.intensity = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float transition = _DissolveY - i.worldPos.y;
				clip(_StartingY + (transition + (tex2D(_DissolveTexture, i.uv)) * _DissolveSize));

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) + _ObjectColor;
				return col * pow(i.intensity + 0.1, 0.5);
			}


			ENDCG
		}
	}
}
