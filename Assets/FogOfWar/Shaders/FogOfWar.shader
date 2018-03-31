﻿Shader "Custom/FogOfWar" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_ShadowMap("Fog of War (rgb)", 2D) = "white" {}
		_LastShadowMap("Last FoW (RGB)", 2D) = "white" {}
		_interpolationValue("interp value", float) = 0
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 2.0

		sampler2D _MainTex;
		sampler2D _ShadowMap;
		sampler2D _LastShadowMap;
		float _interpolationValue;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 s = tex2D(_ShadowMap, IN.uv_MainTex);
			fixed4 ls = tex2D(_LastShadowMap, IN.uv_MainTex);

			s = lerp(ls, s, _interpolationValue);

			o.Albedo = c.rgb * (s.r + s.g) / 2;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Legacy Shaders/Diffuse"
}