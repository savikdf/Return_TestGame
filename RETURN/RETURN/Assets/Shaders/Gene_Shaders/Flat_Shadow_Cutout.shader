Shader "Custom/Gene/Flat_CutOut_Shadow"
{
	Properties
	{
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex("Texture", 2D) = "white"{}
		_Cutoff("Cut the White Shit", Range(0,1)) = .5
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
		uniform float4 _Color;

		uniform fixed _Cutoff;

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

		float4 TexM = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy +  _MainTex_ST.zw);
		fixed atten = LIGHT_ATTENUATION(i);
	
		//Lightning Calculations
		fixed nDotL = saturate(dot(i.normalDir, i.lightDir.xyz));
		//atten += TexM.a;
		//0.25 = _DiffusionThreshold
		fixed diffuseCutoff = saturate((max(0.35, nDotL) - 0.35) * pow((2), 10));

		fixed3 ambientLight = (1 - diffuseCutoff) * _LightColor0.xyz;

		fixed3 diffusionReflection =  (diffuseCutoff) * atten/2;

		fixed3 LightFinal = UNITY_LIGHTMODEL_AMBIENT.xyz + ambientLight + diffusionReflection;

		if(TexM.a < _Cutoff) discard;

					for(int r = 0; r < 25; r++)
					{
						for(int i = 0; i < 15; i++)
							{
								//TexM.a *= i.pos.xy;
							}		
					}

		return fixed4(_Color * TexM * LightFinal, 1.0);
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


		struct v2f
		{
			V2F_SHADOW_CASTER;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
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
