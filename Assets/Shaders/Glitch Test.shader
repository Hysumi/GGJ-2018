Shader "Hidden/Glitch Test"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OffsetX ("X Offset", Float) = 0
		_OffsetY ("Y Offset", Float) = 0
		_SpeedX ("Speed X", Float) = 0
		_SpeedY("Speed Y", Float) = 0
		_Wavy("Wavy", Float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed _OffsetX;
			fixed _OffsetY;
			fixed _SpeedX;
			fixed _SpeedY;
			fixed _Wavy;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(
									_MainTex, float2(
													frac(i.uv.x + (_OffsetX * sin(_Wavy * i.vertex.y) * sin(_Time.y * _SpeedX))), 
													frac(i.uv.y + (_OffsetY * sin(_Wavy * i.vertex.x) *sin(_Time.y * _SpeedY)))
													)
								   );
				return col;
			}
			ENDCG
		}
	}
}
