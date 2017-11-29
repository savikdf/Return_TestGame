Shader "Custom/Gene/PostEffect/SSAO" {
	SubShader
	{
		Pass
		{
		Fog { Mode Off }
			Cull Off
		CGPROGRAM
		#pragma glsl
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		static const float PI = 3.14159265f;

		uniform sampler2D _MainTex;
		uniform float _DepthValue;
		uniform sampler2D _CameraDepthTexture;
		uniform float4 _CameraDepthTexture_ST;
		uniform sampler2D _CameraDepthNormalsTexture;

		float3 randomNormal(float2 tex)
		{
			float noiseX = (frac(sin(dot(tex, float2(15.8989, 76.132) * 1.0)) * 46336.23745));
			float noiseY = (frac(sin(dot(tex, float2(11.9899, 62.223) * 2.0)) * 34748.34744));
			float noiseZ = (frac(sin(dot(tex, float2(13.3238, 63.122) * 3.0)) * 59998.47362));
			
			return normalize(float3(noiseX, noiseY, noiseZ));
		}

		struct v2f
		{
		half4 position : POSITION;
		half4 screenPos : TEXCOORD1;
		};

		v2f vert(appdata_base v)
		{

		v2f o;

		o.position = mul(UNITY_MATRIX_MVP, v.vertex);
		o.screenPos = ComputeScreenPos(o.position);
		return o;
		}

		float4 frag(v2f i) : SV_TARGET
		{
		
		const int samples = 16;
		const float samplerRadius = 0.00004;
		const float strength = 2;
		const float totalStrength = 5.0;
		const float falloffMin = 0.000001;
		const float falloffMax = 0.0006;

		const float3 sampleSphere[16] = 
		{
			float3( 0.2024537f, 0.841204f, -0.9060141f), 
    		float3(-0.2200423f, 0.6282339f,-0.8275437f), 
    		float3( 0.3677659f, 0.1086345f,-0.4466777f), 
    		float3( 0.8775856f, 0.4617546f,-0.6427765f), 
    		float3( 0.7867433f,-0.141479f, -0.1567597f), 
    		float3( 0.4839356f,-0.8253108f,-0.1563844f), 
    		float3( 0.4401554f,-0.4228428f,-0.3300118f), 
    		float3( 0.0019193f,-0.8048455f, 0.0726584f), 
    		float3(-0.7578573f,-0.5583301f, 0.2347527f), 
    		float3(-0.4540417f,-0.252365f, 0.0694318f), 
    		float3(-0.0483353f,-0.2527294f, 0.5924745f), 
    		float3(-0.4192392f, 0.2084218f,-0.3672943f), 
    		float3(-0.8433938f, 0.1451271f, 0.2202872f), 
    		float3(-0.4037157f,-0.8263387f, 0.4698132f), 
    		float3(-0.6657394f, 0.6298575f, 0.6342437f), 
    		float3(-0.0001783f, 0.2834622f, 0.8343929f),

		};

		float3 randNor = randomNormal(i.screenPos.xy);

		float4 MainTex = tex2D(_MainTex, float4(i.screenPos.xy, 0.0, 0.0));

		float4 depth = Linear01Depth(tex2D(_CameraDepthTexture, (i.screenPos)));
		//Normals Arnt unpacked yet
		float3 normal;

		DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy), depth.r, normal);

		float radius = samplerRadius / depth.b;

		float3 centrePos = float3(i.screenPos.xy, depth.a);

		float occ = 0.0;

		//calculate normal direction
		for(int i = 0; i < samples; i++)
			{
				float3 offset = reflect(sampleSphere[i], randNor);
				offset = sign(dot(offset, normal)) * offset;
				offset.y = 1 - -offset.y;

				float3 ray = centrePos + radius * offset;

				if((saturate(ray.x) != ray.x) || (saturate(ray.y) != ray.y)) continue;

				float4 occDepth = Linear01Depth(tex2Dlod(_CameraDepthTexture, float4 (ray.xy, 0.0, 0)));
			
				float3 occNormal;
				DecodeDepthNormal(tex2Dlod(_CameraDepthNormalsTexture, float4(ray.xy, 0.0, 0.0)), occDepth.b, occNormal);

				float depthDiffrence = (centrePos.z - occDepth.r);

				float normalDiffrence = dot(occNormal, normal);

				float normalOcc = (1.0 - saturate(normalDiffrence));

				float depthOcc = step(falloffMin, depthDiffrence) *
				(1.0 - smoothstep(falloffMin, falloffMax, depthDiffrence));

				occ += saturate(depthOcc*normalOcc*strength);
			}

			occ /= samples;
		
			float4 ouput = (saturate(pow(1.0 - occ, totalStrength)));
			return ouput * MainTex;
		}
		 
		ENDCG
		}
	}
}

