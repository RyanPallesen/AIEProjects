Shader "Custom/EdgeDetectionShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Threshold("Threshold", float) = 0.01
        _EdgeColor("Edge color", Color) = (0,0,0,1)
        _ShadowThreshold("Shadow Threshold", float) = 0.01
        _ShadowEdgeColor("Shadow color", Color) = (0,0,0,1)
        _BackgroundColor("Background color", Color) = (0,0,0,1)
    }
        SubShader
        {
            // No culling or depth
            Cull Off ZWrite Off ZTest Always

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

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

                sampler2D _CameraDepthNormalsTexture;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                float _Threshold;
                fixed4 _EdgeColor;
                fixed4 _ShadowEdgeColor;
                fixed4 _BackgroundColor;
                float _ShadowThreshold;

                float4 GetPixelValue(in float2 uv) {
                    half3 normal;
                    float depth;
                    DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv), depth, normal);
                    return fixed4(normal, depth);
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 orValue = tex2D(_MainTex, i.uv);
                    float2 offsets[8] = {
                        float2(-1, -1),
                        float2(-1, 0),
                        float2(-1, 1),
                        float2(0, -1),
                        float2(0, 1),
                        float2(1, -1),
                        float2(1, 0),
                        float2(1, 1)
                    };

                    fixed4 sampledValue = fixed4(0,0,0,0);
                    float maxDifference = 0;

                    for (int j = 0; j < 8; j++) {

                        float4 currentValue = tex2D(_MainTex, i.uv + offsets[j] * _MainTex_TexelSize.xy);
                        sampledValue += tex2D(_MainTex, i.uv + offsets[j] * _MainTex_TexelSize.xy);

                        if (length(orValue - currentValue) > maxDifference)
                        {
                            maxDifference = length(orValue - currentValue);
                        }
                    }

                    sampledValue /= 8;
                    if (maxDifference > 1 - (_Threshold / 100))
                    {
                        return _EdgeColor;
                    }

                    if (length(sampledValue) < _ShadowThreshold / 100)
                    {
                        return _ShadowEdgeColor;
                    }

                    return _BackgroundColor;
                }
                ENDCG
            }
        }
}