// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.texture[0]' with 'UNITY_MATRIX_TEXTURE0'
// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'


Shader "Glitch/Glitcher Look Through"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Steps ("Steps", Int) = 0
		_Amplitude("Amplitude", Float) = 0
		_Height("Height", Float) = 0
		_Speed("Speed", Float) = 0
		_Transparency("Transparency", Range(0,1)) = 1.0
		_Radius("UV Radius", Range(0,0.8)) = 0.8
		_GlitchSpeed("Glitch Speed", Float) = 1.0
		
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _GrabTexture;
			float4 _MainTex_ST;
			int _Steps;
			float _Amplitude;
			float _Height;
			float _Speed;
			float _Transparency;
			float _Radius;
			float _GlitchSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;				
				v.vertex.x += sin(_Time.y * _Speed + v.vertex.z * _Height) * _Amplitude;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				int steps = _Steps;
				
				fixed4 col = tex2D(_MainTex, i.uv);
				
				
				col.rgb = fixed3( frac(i.uv.y * steps + (_GlitchSpeed * _Time.y)), frac(steps * i.uv.x + (_GlitchSpeed * _SinTime.w) * i.uv.y), frac(sin(i.uv.x * steps)));
				col.a = _Transparency;
				clip((sqrt(pow(i.uv.x-0.5, 2) + pow(i.uv.y-0.5, 2))-_Radius)*-1);
				return col;
			}
			ENDCG
		}
	}
}
