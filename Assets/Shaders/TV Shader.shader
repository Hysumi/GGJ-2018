Shader "Hidden/TV Shader"
{
	//Does white noise and image distortion
	
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Offset("Distortion Offset", Float) = 0.0
		_NoiseBlend("Noise Blend", Range(0,1)) = 0.0
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
			
			
			
			float rand(fixed2 co){
				return frac(sin(dot(co.xy * _Time.y ,fixed2(12.9898,78.233))) * 43758.5453);
			}

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
				//o.uv.x +=  saturate(((-4 * pow(o.uv.x -0.5, 2)) + 1) * _Offset);
				o.uv.y += saturate(((-4 * pow(o.uv.y -0.5, 2)) + 1) * _Offset);
				return o;
			}
			
			sampler2D _MainTex;
			fixed _NoiseBlend;

			fixed4 frag (v2f i) : SV_Target
			{
				half aux = sqrt(2)/2;
				fixed2 relUv = i.uv -  fixed2(0.5,0.5);
				half dist = length(relUv);
				
				fixed newDist = dist - (((-4*pow(((2*dist)/sqrt(2)) - 0.5 ,2)) + 1) * _Offset);
				
				fixed2 Uv = fixed2(0.5,0.5) + normalize(relUv) * newDist;
				
				fixed4 col = tex2D(_MainTex, Uv);
				
				half lum = rand(i.uv);
				
				col = lerp(col, fixed4(lum, lum, lum, 1), _NoiseBlend);
				
				return col;
			}
			ENDCG
		}
	}
}
