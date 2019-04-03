Shader "Unlit/Hologram"
{
    Properties
    {
        _Size ("Grid Size", Float) = 1.0
        _GridColor ("Grid Colour", Color) = (1.0, 1.0, 1.0, 1.0)
        _BaseColor ("Base Colour", Color) = (0.3, 0.3, 0.3, 0.3)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        
        /*
        Pass
        {
            Tags { "RenderQueue"="1999" }
            Cull Front
            CGPROGRAM
            
            ENDCG
        }
        */
        

        Pass
        {
            Tags { "RenderQueue"="2001" }
            Cull Back
            CGPROGRAM
            
            ENDCG
        }

        CGINCLUDE
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 localPos : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float _Size;
            fixed4 _GridColor;
            fixed4 _BaseColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.localPos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.localPos /= _Size;
                i.localPos = abs(i.localPos);
                i.localPos %= 1;

                float3 gridIntensities = i.localPos.xyz;
                for(int i = 0; i < 3; i++) {
                    if(gridIntensities[i] > 0.5) gridIntensities[i] = 1 - gridIntensities[i];

                    if(gridIntensities[i] > 0.1) gridIntensities[i] = 0;
                    else gridIntensities[i] = (0.1 - gridIntensities[i]) * 10;
                }

                float intensity = max(gridIntensities.r, max(gridIntensities.g, gridIntensities.b));


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return (intensity * _GridColor) + ((1 - intensity) * _BaseColor);
            }
        ENDCG
    }
}
