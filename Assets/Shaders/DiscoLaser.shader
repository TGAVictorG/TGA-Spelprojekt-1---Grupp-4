Shader "Unlit/DiscoLaser"
{
    Properties
    {
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _NoiseTex("Noise Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv: TEXCOORD2;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.uv = worldPos.xz * 0.4 + float2(_Time.x, _Time.x);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 a = tex2D(_NoiseTex, i.uv);
                a = 1.0 - a;
                return fixed4(_Color.rgb, _Color.a * (a.r * a.r));
            }
            ENDCG
        }
    }
}
