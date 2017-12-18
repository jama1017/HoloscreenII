Shader "Custom/Pet" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_GlossMap ("Gloss (R)", 2D) = "black" {}
		_GlossScale ("GlossScale", Range(0, 100)) = 0
		_Specular ("Specular", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GlossMap;
		sampler2D _BumpMap;
		float _Specular;
		float _GlossScale;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_GlossMap;
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf(Input IN, inout SurfaceOutput o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Specular = _Specular;
			o.Gloss = tex2D(_GlossMap, IN.uv_GlossMap).r * _GlossScale;
			_SpecColor = float4(1, 1, 1, 1);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
