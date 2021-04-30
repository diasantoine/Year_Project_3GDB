// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		_WaterColor("WaterColor", Color) = (0.06460939,0.1320755,0.01682094,0)
		_MoveSpeed("MoveSpeed", Float) = 0.1
		_WaveAmplitude("WaveAmplitude", Float) = 0
		[HDR]_WaveColor("WaveColor", Color) = (0.002709357,0.1886792,0,0)
		_WaveSpeed("WaveSpeed", Float) = 0.5
		_WaveScale("WaveScale", Float) = 5
		_WavePower("WavePower", Float) = 0.05
		_FoamColor("FoamColor", Color) = (0.1446341,0.2358491,0.1012371,0)
		_FoamDistance("FoamDistance", Float) = 0
		_FoamPower("FoamPower", Range( 0 , 1)) = 0
		_CloudColor("CloudColor", Color) = (0.1527808,0.3490566,0.07079922,0)
		_CloudScale("CloudScale", Float) = 2
		_CloudSpeed("CloudSpeed", Float) = 0
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _MoveSpeed;
		uniform float _WaveAmplitude;
		uniform float4 _WaterColor;
		uniform float4 _WaveColor;
		uniform float _WaveScale;
		uniform float _WaveSpeed;
		uniform float _WavePower;
		uniform float _CloudSpeed;
		uniform float _CloudScale;
		uniform float4 _CloudColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FoamDistance;
		uniform float _FoamPower;
		uniform float4 _FoamColor;
		uniform float _EdgeLength;


		//https://www.shadertoy.com/view/XdXGW8
		float2 GradientNoiseDir( float2 x )
		{
			const float2 k = float2( 0.3183099, 0.3678794 );
			x = x * k + k.yx;
			return -1.0 + 2.0 * frac( 16.0 * k * frac( x.x * x.y * ( x.x + x.y ) ) );
		}
		
		float GradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 i = floor( p );
			float2 f = frac( p );
			float2 u = f * f * ( 3.0 - 2.0 * f );
			return lerp( lerp( dot( GradientNoiseDir( i + float2( 0.0, 0.0 ) ), f - float2( 0.0, 0.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 0.0 ) ), f - float2( 1.0, 0.0 ) ), u.x ),
					lerp( dot( GradientNoiseDir( i + float2( 0.0, 1.0 ) ), f - float2( 0.0, 1.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 1.0 ) ), f - float2( 1.0, 1.0 ) ), u.x ), u.y );
		}


		float2 voronoihash6( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi6( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash6( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 temp_cast_0 = ((0.0*1.0 + ( _Time.y * _MoveSpeed ))).xx;
			float2 uv_TexCoord37 = v.texcoord.xy + temp_cast_0;
			float gradientNoise34 = GradientNoise(uv_TexCoord37,_WaveAmplitude);
			gradientNoise34 = gradientNoise34*0.5 + 0.5;
			float3 objToWorld44 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float MoveVertexY77 = ( ( ase_vertexNormal.y * gradientNoise34 ) + objToWorld44.y );
			float3 temp_cast_1 = (MoveVertexY77).xxx;
			v.vertex.xyz += temp_cast_1;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float time6 = ( _Time.y * _WaveSpeed );
			float2 temp_output_1_0_g2 = i.uv_texcoord;
			float2 temp_output_11_0_g2 = ( temp_output_1_0_g2 - float2( 0.5,0.5 ) );
			float2 break18_g2 = temp_output_11_0_g2;
			float2 appendResult19_g2 = (float2(break18_g2.y , -break18_g2.x));
			float dotResult12_g2 = dot( temp_output_11_0_g2 , temp_output_11_0_g2 );
			float2 coords6 = ( temp_output_1_0_g2 + ( appendResult19_g2 * ( dotResult12_g2 * float2( 1,1 ) ) ) + float2( 0,0 ) ) * _WaveScale;
			float2 id6 = 0;
			float2 uv6 = 0;
			float voroi6 = voronoi6( coords6, time6, id6, uv6, 0 );
			float simplePerlin2D107 = snoise( (i.uv_texcoord*1.0 + ( float2( 0.1,0 ) * _Time.y * _CloudSpeed ))*_CloudScale );
			simplePerlin2D107 = simplePerlin2D107*0.5 + 0.5;
			float simplePerlin2D108 = snoise( (i.uv_texcoord*1.0 + ( _Time.y * float2( 0,0.1 ) * _CloudSpeed ))*_CloudScale );
			simplePerlin2D108 = simplePerlin2D108*0.5 + 0.5;
			float blendOpSrc97 = simplePerlin2D107;
			float blendOpDest97 = simplePerlin2D108;
			float4 blendOpSrc110 = ( _WaterColor + ( _WaveColor * pow( voroi6 , _WavePower ) ) );
			float4 blendOpDest110 = ( ( saturate( ( blendOpSrc97 * blendOpDest97 ) )) + _CloudColor );
			float4 WaterColor133 = ( saturate( min( blendOpSrc110 , blendOpDest110 ) ));
			o.Albedo = WaterColor133.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth80 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth80 = abs( ( screenDepth80 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamDistance ) );
			float clampResult150 = clamp( ( ( 1.0 - distanceDepth80 ) * _FoamPower ) , 0.0 , 1.0 );
			float4 Foam151 = ( clampResult150 * _FoamColor );
			o.Emission = Foam151.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
293;189;1374;655;1700.871;-508.6691;1.37603;True;True
Node;AmplifyShaderEditor.CommentaryNode;78;-2216.373,642.6547;Inherit;False;2154.74;616.0648;Comment;12;77;30;32;31;29;37;47;35;34;36;44;38;MoveWater;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;100;-3495.252,-1338.777;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;104;-3230.198,-1496.334;Inherit;False;Constant;_Vector5;Vector 5;10;0;Create;True;0;0;False;0;False;0.1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;114;-3185.993,-1340.257;Inherit;False;Property;_CloudSpeed;CloudSpeed;12;0;Create;True;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;102;-3253.367,-1219.892;Inherit;False;Constant;_Vector3;Vector 3;10;0;Create;True;0;0;False;0;False;0,0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;32;-2166.373,994.012;Inherit;False;Property;_MoveSpeed;MoveSpeed;1;0;Create;True;0;0;False;0;False;0.1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;30;-2160.905,888.72;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-3082.922,-567.062;Inherit;False;Property;_WaveSpeed;WaveSpeed;4;0;Create;True;0;0;False;0;False;0.5;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;25;-3087.933,-654.8359;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;24;-3052.623,-798.8242;Inherit;False;Constant;_Vector4;Vector 4;5;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-3125.324,-924.4919;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;18;-2877.771,-829.0833;Inherit;False;Radial Shear;-1;;2;c6dc9fc7fa9b08c4d95138f2ae88b526;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-2907.661,-655.9901;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2359.135,-67.13081;Inherit;False;Property;_FoamDistance;FoamDistance;8;0;Create;True;0;0;False;0;False;0;0.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2811.834,-534.5692;Inherit;False;Property;_WaveScale;WaveScale;5;0;Create;True;0;0;False;0;False;5;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-3018.36,-1490.382;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-3031.604,-1234.098;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;106;-2852.608,-1411.713;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1956.653,939.7495;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;29;-1776.418,860.9056;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;6;-2628.348,-781.1469;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;7.89;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;109;-2550.77,-1389.265;Inherit;False;Property;_CloudScale;CloudScale;11;0;Create;True;0;0;False;0;False;2;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;80;-2134.391,-117.5646;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;93;-2619.844,-1542.398;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;95;-2621.554,-1293.474;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2547.668,-500.2302;Inherit;False;Property;_WavePower;WavePower;6;0;Create;True;0;0;False;0;False;0.05;3.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1553.658,1151.819;Inherit;False;Property;_WaveAmplitude;WaveAmplitude;2;0;Create;True;0;0;False;0;False;0;3.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;135;-1859.425,-125.4742;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-2386.141,-779.3683;Inherit;False;Property;_WaveColor;WaveColor;3;1;[HDR];Create;True;0;0;False;0;False;0.002709357,0.1886792,0,0;0.3236214,0.6132076,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;11;-2365.54,-587.4714;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-1544.547,915.7349;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;84;-2063.256,-0.3722458;Inherit;False;Property;_FoamPower;FoamPower;9;0;Create;True;0;0;False;0;False;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;107;-2360.87,-1551.211;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;108;-2363.39,-1291.22;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;97;-2074.515,-1551.339;Inherit;True;Multiply;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;-2136.274,-928.206;Inherit;False;Property;_WaterColor;WaterColor;0;0;Create;True;0;0;False;0;False;0.06460939,0.1320755,0.01682094,0;0.06944063,0.3018868,0.03844785,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1693.043,-129.2241;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;116;-2031.22,-1290.328;Inherit;False;Property;_CloudColor;CloudColor;10;0;Create;True;0;0;False;0;False;0.1527808,0.3490566,0.07079922,0;0.01682093,0.1698113,0.0317061,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2089.643,-686.5985;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;34;-1133.31,951.2845;Inherit;True;Gradient;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;35;-1220.644,714.3048;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-1854.563,-744.9968;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;153;-1351.717,138.7388;Inherit;False;Property;_FoamColor;FoamColor;7;0;Create;True;0;0;False;0;False;0.1446341,0.2358491,0.1012371,0;0.3176471,0.3490196,0.1529412,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-891.7369,749.4818;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-1720.049,-1423.032;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;150;-1389.055,-145.206;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;44;-791.6434,1044.353;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-571.3489,745.6909;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;110;-1513.983,-796.6685;Inherit;True;Darken;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;154;-1050.196,-125.824;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;133;-1207.976,-729.869;Inherit;False;WaterColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-760.5225,-171.017;Inherit;False;Foam;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;77;-285.632,700.8207;Inherit;False;MoveVertexY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;149;299.1879,-93.2287;Inherit;False;133;WaterColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;147;298.6636,44.6527;Inherit;False;151;Foam;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;79;318.9598,293.2707;Inherit;False;77;MoveVertexY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;586.4448,-47.64548;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;13;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;1;22;0
WireConnection;18;3;24;0
WireConnection;8;0;25;0
WireConnection;8;1;9;0
WireConnection;103;0;104;0
WireConnection;103;1;100;0
WireConnection;103;2;114;0
WireConnection;101;0;100;0
WireConnection;101;1;102;0
WireConnection;101;2;114;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;29;2;31;0
WireConnection;6;0;18;0
WireConnection;6;1;8;0
WireConnection;6;2;10;0
WireConnection;80;0;81;0
WireConnection;93;0;106;0
WireConnection;93;2;103;0
WireConnection;95;0;106;0
WireConnection;95;2;101;0
WireConnection;135;0;80;0
WireConnection;11;0;6;0
WireConnection;11;1;12;0
WireConnection;37;1;29;0
WireConnection;107;0;93;0
WireConnection;107;1;109;0
WireConnection;108;0;95;0
WireConnection;108;1;109;0
WireConnection;97;0;107;0
WireConnection;97;1;108;0
WireConnection;83;0;135;0
WireConnection;83;1;84;0
WireConnection;15;0;16;0
WireConnection;15;1;11;0
WireConnection;34;0;37;0
WireConnection;34;1;47;0
WireConnection;113;0;73;0
WireConnection;113;1;15;0
WireConnection;36;0;35;2
WireConnection;36;1;34;0
WireConnection;115;0;97;0
WireConnection;115;1;116;0
WireConnection;150;0;83;0
WireConnection;38;0;36;0
WireConnection;38;1;44;2
WireConnection;110;0;113;0
WireConnection;110;1;115;0
WireConnection;154;0;150;0
WireConnection;154;1;153;0
WireConnection;133;0;110;0
WireConnection;151;0;154;0
WireConnection;77;0;38;0
WireConnection;0;0;149;0
WireConnection;0;2;147;0
WireConnection;0;11;79;0
ASEEND*/
//CHKSM=A17332ECE2F1A527BB6283D166CF53AE87A67B27