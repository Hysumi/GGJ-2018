﻿Shader "Hidden/Transition"
{
	//Does white noise and image distortion
	
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Offset("Distortion Offset", Float) = 0.0
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
			
			fixed _Offset;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
								
				o.uv = v.uv;
				o.uv.x +=  saturate(((-4 * pow(o.uv.x -0.5, 2)) + 1) * _Offset);
				//o.uv.y += saturate(((-4 * pow(o.uv.y -0.5, 2)) + 1) * _Offset);
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, fixed2(i.uv.x + ((-4 * pow(i.uv.x -0.75, 2)) + 1 * _Offset), i.uv.y + (((-4 * pow(i.uv.y -0.75, 2)) + 1) * _Offset)));
				return col;
			}
			ENDCG
		}
	}
}
