Shader "Custom/Fog2" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FogColor ("Fog Color (RGB)", 2D) = "gray" {}
		_FogStart ("Fog Start", Float) = 0
		_FogEnd ("Fog End", Float) = 300
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert finalcolor:mycolor vertex:myvert nofog

		sampler2D _MainTex;
		sampler2D _FogColor;
		uniform half _FogStart;
		uniform half _FogEnd;

		struct Input {
			float2 uv_MainTex;
			half fog;
		};

		void myvert(inout appdata_full v, out Input data) {
			UNITY_INITIALIZE_OUTPUT(Input,data);
			float pos = length(mul(UNITY_MATRIX_MV, v.vertex).xyz);
			float diff = _FogEnd - _FogStart;
			float invDiff = 1.0f / diff;
			data.fog = 1.0 - clamp((_FogEnd - pos) * invDiff, 0.0, 1.0);
		}

		void mycolor(Input IN, SurfaceOutput o, inout fixed4 color) {
			half3 fogColor = tex2D(_FogColor, half2(IN.fog, 0.5));
			#ifdef UNITY_PASS_FORWARDADD
			fogColor = 0;
			#endif
			//color.rgb = lerp(color.rgb, fogColor, IN.fog);
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
