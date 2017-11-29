// By Tim Volp
// Updated 04/04/15

Shader "Sand/Sand-Bumped" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Grain ("Grain", 2D) = "gray" {}
		_Graininess ("Graininess", Float) = 10.0
		_Shininess ("Shininess", Float) = 200.0
		_Reflectance ("Reflectance", Float) = 5.0
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Sand

		sampler2D _Grain;
		half _Graininess;
		half _Shininess;
		half _Reflectance;

		// Custom lighting model
		inline half4 LightingSand(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
			#endif

			viewDir = normalize(viewDir);

			// Calculate lighting from angle between normal and light direction
			half NdotL = saturate(dot(s.Normal, lightDir));

			// Calculate reflected light direction from light direction and normal
			lightDir = reflect(lightDir, s.Normal);

			// Calculate specular lighting from absolute angle between reflected light direction and view direction
			half specular = saturate(abs(dot(lightDir, viewDir)));

			// New detail based on detail texture and graininess
			half detail = tex2D(_Grain, half2(fmod(viewDir.z * _Graininess, 1.0), fmod(s.Normal.x * _Graininess, 1.0)));

			// New specular based on shininess and detail
			specular = pow(specular, _Shininess) * floor(detail * _Reflectance);

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb  * (atten * 2) * (NdotL + specular);
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		sampler2D _BumpMap;
		half4 _Color;

		struct Input {
			half2 uv_MainTex;
			half2 uv_BumpMap;
			half2 uv_Grain;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color * c.a;
			//o.Albedo *= tex2D (_Grain, IN.uv_Grain).rgb * 1.5;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
