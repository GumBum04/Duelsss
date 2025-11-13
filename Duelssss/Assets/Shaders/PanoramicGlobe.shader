Shader "Unlit/PanoramicGlobe"
{
    Properties
    {
        _MainTex ("Equirectangular Texture", 2D) = "white" {}
        _Rotation ("Horizontal Rotation", Range(0, 6.283)) = 0
        _VerticalOffset ("Vertical Look", Range(-1, 1)) = 0
        _VerticalWarpStrength ("Vertical Warp Strength", Range(0,0.2)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Rotation;
            float _VerticalOffset;
            float _VerticalWarpStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2.0 - 1.0; // remap 0–1 UVs to -1–1 range
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert screen UV to spherical coordinates
                float2 uv = i.uv;
                float longitude = atan2(uv.x, 1.0);
                float latitude  = uv.y * (3.14159 / 2.0);

                // Apply rotation + vertical offset
                longitude += _Rotation;
                latitude  += _VerticalOffset * (3.14159 / 2.0);

                // Convert spherical coords back to equirectangular UVs
                float2 sphereUV;
                sphereUV.x = frac((longitude / (2.0 * 3.14159)) + 0.5);
                sphereUV.y = 0.5 + (latitude / 3.14159);

                // --- Vertical Warp
                float centeredY = sphereUV.y - 0.5; // distance from vertical center
                sphereUV.x += _VerticalWarpStrength * (centeredY * centeredY) * (centeredY > 0 ? -1 : 1);

                return tex2D(_MainTex, sphereUV);
            }
            ENDCG
        }
    }
}
