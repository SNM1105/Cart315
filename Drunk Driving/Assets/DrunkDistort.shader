
Shader "Custom/DrunkDistortion"
{
    Properties
    {
        _Intensity ("Intensity", Float) = 0
        _Speed ("Wave Speed", Float) = 2
        _Frequency ("Wave Frequency", Float) = 10
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _Intensity;
            float _Speed;
            float _Frequency;

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                float time = _Time.y * _Speed;

                // Water-like distortion
                uv.x += sin(uv.y * _Frequency + time)
                        * 0.08 * _Intensity;

                uv.y += cos(uv.x * _Frequency + time)
                        * 0.05 * _Intensity;

                // Colour separation
                float offset = _Intensity * 0.008;

                half4 color;

                color.r = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv + float2(offset, 0)
                ).r;

                color.g = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv
                ).g;

                color.b = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv - float2(offset, 0)
                ).b;

                color.a = 1;

                // Soft blur
                float blur = _Intensity * 0.004;

                half4 blurred = color;

                blurred += SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv + float2(blur, 0)
                );

                blurred += SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv - float2(blur, 0)
                );

                blurred += SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv + float2(0, blur)
                );

                blurred += SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv - float2(0, blur)
                );

                blurred /= 5.0;

                return lerp(
                    color,
                    blurred,
                    saturate(_Intensity * 0.5)
                );
            }

            ENDHLSL
        }
    }
}
