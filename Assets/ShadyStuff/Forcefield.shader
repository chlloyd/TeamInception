Shader "Unlit/Forcefield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _RippleOrigins[10];

            float _CurrentTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex)
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 netOffset = float2(0, 0);
                float2 l = float2(0, 0);
                float t = 0;
                float d = 0;
                for(int itr = 0; itr < 10; itr++) {
                    t = _CurrentTime - _RippleOrigins[itr].w;
                    if(t > 2.0) continue;

                    l = i.worldPos.zy - _RippleOrigins[itr].zy;
                    d = t - length(l);
                    if(d < 0.0) d = 1.0;
                    netOffset += l * ((1.0 - d) / 60.0) * (2.0 - t);
                }

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv + float2(netOffset.x, -netOffset.y));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col + lerp(float4(0.2, 0.5, 1.0, 1.0), float4(0.5, 0.2, 1.0, 1.0), sin(_Time.x * 5)) * length(netOffset) * 5;
                //return length(_RippleOrigins[0].xyz - i.worldPos.xyz);
                //return _Time.y % 1.0;
            }
            ENDCG
        }
    }
}
