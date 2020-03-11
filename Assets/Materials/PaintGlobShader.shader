Shader "Custom/PaintGlobShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_NoiseScale ("Noise Scale", Range(0,10)) = 1.0
		_NoiseSpeed ("Noise Speed", Range(0,10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow
		#pragma vertex vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
			float3 worldPos;
        };

		struct v2f {
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;
		};

        half _Glossiness;
        half _Metallic;
		half _NoiseScale;
		half _NoiseSpeed;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		float hash(float n) {
			return frac(sin(n) * 43758.5453);
		}

		float noise(float3 x) {
			float3 p = floor(x);
			float3 f = frac(x);

			f = f * f * (3.0 - 2.0 * f);
			float n = p.x + p.y * 57.0 + 113.0 * p.z;

			return lerp(	lerp(	lerp(hash(n + 0.0),		hash(n + 1.0),		f.x),
									lerp(hash(n + 57.0),	hash(n + 58.0),		f.x), f.y),
							lerp(	lerp(hash(n + 113.0),	hash(n + 114.0),	f.x),
									lerp(hash(n + 170.0),	hash(n + 171.0),	f.x), f.y), f.z);
		}

		void vert(inout appdata_full v) {
			float3 worldPos = v.vertex.xyz * _NoiseScale;
			float3 timePos = float3(_Time.y, _Time.y, _Time.y) * _NoiseSpeed;
			v.vertex.xyz += v.normal * 0.6 * length(float3(noise(worldPos + timePos), noise(worldPos * -1.0 + timePos), noise(worldPos + timePos * -1.0)));
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {			
			float3 worldPos = IN.worldPos * _NoiseScale;
			float3 timePos = float3(_Time.y, _Time.y, _Time.y) * _NoiseSpeed;
			float4 noiseVal = float4(noise(worldPos + timePos), noise(worldPos * -1.0 + timePos), noise(worldPos + timePos * -1.0), 1.0);

            // Albedo comes from a texture tinted by color
            fixed4 c = _Color * noiseVal;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
