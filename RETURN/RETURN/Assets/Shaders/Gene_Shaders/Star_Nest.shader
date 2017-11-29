Shader "Custom/Gene/Star_Nest"
{

	SubShader
	{
	
		Pass
		{
		

		Cull Off
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		uniform sampler2D oldImage;
		#define iterations  17
		#define formuparam  0.53

		#define volsteps  20
		#define tile 0.850

		#define stepsize  0.1

		#define brightness  0.0018
		#define darkmatter  0
		#define distfading  0.730
		#define saturation 0.850
		#define zoom  2

		struct v2f
		{
		half4 position : SV_POSITION;
		half4 texcoord : TEXCOORD0;
		half3 viewDir : TEXCOORD1;
		half3 normalDir : NORMAL;
		};
		
		float mod(float x, float y)
		{
		 return x - y * floor(x/y);
		}

		v2f vert(appdata_base v)
		{
		v2f o;
		o.position = mul(UNITY_MATRIX_MVP, v.vertex);
		half4 posWorld = mul(_Object2World, v.vertex);
		o.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
		o.texcoord = v.texcoord;
		o.normalDir = normalize(mul(half4(v.normal, 0.0), _World2Object).xyz);
		return o;
		}
			fixed4 frag(v2f i) : SV_Target 
			{
				
                float2 uv = i.texcoord.xy/_ScreenParams.xy -2;
                //uv.y *= _ScreenParams.x/_ScreenParams.y ;
				float time = _Time.x/100;
				float3 from = float3(1, 0.5, 0.5);
				from += float3(time, time, -2.);
                // Background				
				//float3 dir = float3(uv * zoom , 1.0);//float3(uv * zoom, 1.0);
				float3 dir = float3(uv * zoom, 1);

				float s = 0.1;
				float fade = 0.01;

				fixed4 outColour = fixed4(0,0,0,1);

					//volsteps = 20
					for(int r = 0; r < volsteps; r++)
					{
						float3 p = (from + - i.viewDir) + s * dir * 0.5;
						p = abs(float3(tile, tile, tile) - (p % float3(tile * 2, tile * 2, tile * 2)));

						float prevlen = 0.0;
						float a = 0.0;
						float pa = 0;

							for(int i = 0; i < iterations; i++)
							{
								p = abs(p)/ dot(p, p) + (-formuparam);
								a += abs(length(p) - pa);
								pa = length(p);
							}
						
						 float dm = max(0, darkmatter - a * a * .001);
						  a *= a * a;
						 //Coloring based on the distance
						 if(r > 6) fade *= 1 - dm;

						 outColour.rgb += fade;
						 outColour.rgb += float3(s, abs(s * s), s* s * s * s * s * s) * a * brightness * fade;
						 fade *= distfading;
						 s += stepsize;
					}

					  outColour.rgb = lerp(float3(length(outColour.rgb),length(outColour.rgb),length(outColour.rgb)), outColour.rgb, saturation);

                return outColour;
            }

		ENDCG
		}
	}

}
