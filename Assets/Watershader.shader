Shader "Custom/Transparent" {
     Properties{
          _Color("Color", Color) = (1,1,1,1)
          _Color2("Color2", Color) = (1,1,1,1)
          _MainTex("Albedo (RGB)", 2D) = "white" {}
     _Cutoff("Cutoff", Range(0,1)) = 0.5
          _Amount("Extrusion Amount", Range(-1,1)) = 0.5
     }
          SubShader{
          Tags{ "Queue" = "Transparent"  "RenderType" = "Transparent" }
          LOD 200

          CGPROGRAM
#pragma surface surf Lambert alpha:fade vertex:vert
          sampler2D _MainTex;
     fixed4 _Color;
     fixed4 _Color2;
     float _Cutoff;
     float _Amount;


     struct Input
     {
          float2 uv_MainTex;
          half height;
     };

     void vert(inout appdata_full v, out Input data)
     {
          UNITY_INITIALIZE_OUTPUT(Input, data);
          v.vertex.xyz += v.normal * _Amount;
          data.height = v.vertex.y;
     }

     void surf(Input IN, inout SurfaceOutput o)
     {
          int h_c = 50;
          fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
          o.Albedo = (1 - IN.height / h_c)*c.rgb + (IN.height / h_c)*_Color2;
          o.Alpha = _Color.a;
          clip(c.a - _Cutoff);
     }
     ENDCG
     }
          FallBack "Diffuse"
}