Shader "Custom/Gene/Flat_TextureAO_Shadow"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white"  {}
		_AOTex("Ambient Occlusion", 2D) = "white" {}
		_MixValue("Texture Mix Value", Range(0,1)) = 0.5
		_SpecDiffusion("Specular Diffusion", Range(0, 0.99)) = 0.0
		_Scale("Scale", float) = 1.0
		_Color("Color Tint", Color) = (1.0,1.0,1.0,1.0)
	}

	SubShader
	{
		Pass
		{

		Tags{"LightMode" = "ForwardBase"}

		Cull Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fwdbase
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;

		uniform fixed _MixValue;

		uniform sampler2D _AOTex;
		uniform float4	_AOTex_ST;

		uniform fixed _SpecDiffusion;

		uniform float4 _Color;
		//unity defined veriable
		uniform half4 _LightColor0;

		struct vertexInput
		{
		half4 vertex : POSITION;
		half4 texcoord : TEXCOORD0;
		half3 normal : NORMAL;
		};

		struct vertexOutput
		{
		half4 pos : SV_POSITION;
		fixed3 normalDir : TEXCOORD0;
		half4 tex : TEXCOORD1;
		fixed4 lightDir : TEXCOORD2;
		fixed3 viewDir : TEXCOORD3;
		LIGHTING_COORDS(4,5)
		};

		vertexOutput vert(vertexInput v)
		{

		vertexOutput o;

		o.normalDir = normalize(mul(half4(v.normal, 0.0), _World2Object).xyz);

		o.tex = v.texcoord;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		half4 posWorld = mul(_Object2World, v.vertex);

		o.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);

		half3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - posWorld.xyz;

		o.lightDir = fixed4(normalize(lerp(_WorldSpaceLightPos0.xyz, fragmentToLightSource, _WorldSpaceLightPos0.w)),
							lerp(1.0, 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w));
		
		TRANSFER_VERTEX_TO_FRAGMENT(o);

		return o;
		}

		fixed4 frag(vertexOutput i) : COLOR
		{

		fixed atten = LIGHT_ATTENUATION(i);

		//Texture Unpack
		float4 TexM = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy +  _MainTex_ST.zw);
		float4 TexAO = tex2D(_AOTex, i.tex.xy * _AOTex_ST.xy + _AOTex_ST.zw);

		//Lightning Calculations
		fixed nDotL = saturate(dot(i.normalDir, i.lightDir.xyz)) * atten;

		//0.25 = _DiffusionThreshold
		fixed diffuseCutoff = saturate((max(0.35, nDotL) - 0.35) * pow((2), 10));

		fixed3 ambientLight = (1 - diffuseCutoff) * saturate(_LightColor0/12);

		fixed3 diffusionReflection =  (diffuseCutoff * TexAO.a);

		fixed3 LightFinal = ambientLight + diffusionReflection;

		fixed4 TexFinal = saturate((TexAO * _LightColor0/2));
		TexM.a = TexFinal.a;
		return fixed4(((TexM) *_Color) * LightFinal, 1.0);
		}

		ENDCG
		}

		Pass
		{

		Name "ShadowCaster"
		Tags{"LightMode" = "ShadowCaster"}

		Fog{Mode Off}
		ZWrite On Ztest Less Cull Off
		Offset[_ShadowBias], [_ShadowBiasSlope]

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_shadowcaster
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"

		uniform float _Scale;

		struct v2f
		{
			V2F_SHADOW_CASTER;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			v.vertex.xyz *= _Scale; //?? why 0_o?
			TRANSFER_SHADOW_CASTER(o);
			return o;
		}

		float4 frag(v2f i) : COLOR
		{
			SHADOW_CASTER_FRAGMENT(i);
		}

		ENDCG

		}

		Pass
		{
			Name "ShadowCollector"
			Tags{"LightMode" = "ShadowCollector"}

			Fog{Mode Off}
			ZWrite On Ztest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector

			#define SHADOW_COLLECTOR_PASS
			#include "UnityCG.cginc"

			uniform float _Scale;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				V2F_SHADOW_COLLECTOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				v.vertex.xyz *= _Scale;
				TRANSFER_SHADOW_COLLECTOR(o)
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}

			ENDCG
		}
	}

	FallBack Off
}
