Shader "Hidden/BnW"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_bwBlend ("Black and White Blend", Range(0,1)) = 0.0
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
			fixed _bwBlend;

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c = tex2D(_MainTex, i.uv);
				fixed lum = c.r*.3 + c.g*.59 + c.b*.11;
				fixed3 bw = fixed3( lum, lum, lum ); 
			 
				fixed4 result = c;
				result.rgb = lerp(c.rgb, bw, _bwBlend);
				return result;
			}
			ENDCG
		}
	}
}
