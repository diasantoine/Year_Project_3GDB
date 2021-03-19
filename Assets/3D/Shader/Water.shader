// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		_WaterColor("WaterColor", Color) = (0.06460939,0.1320755,0.01682094,0)
		[HDR]_WaveColor("WaveColor", Color) = (0.002709357,0.1886792,0,0)
		_WaveSpeed("WaveSpeed", Float) = 0.5
		_WaveScale("WaveScale", Float) = 5
		_WavePower("WavePower", Float) = 0.05
		_FoamColor("FoamColor", Color) = (0.0602065,0.254902,0,0)
		_FoamDistance("FoamDistance", Float) = 0
		_FoamPower("FoamPower", Range( 0 , 1)) = 0
		_CloudColor("CloudColor", Color) = (0.1527808,0.3490566,0.07079922,0)
		_CloudScale("CloudScale", Float) = 2
		_CloudSpeed("CloudSpeed", Float) = 0
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
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

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
			float4 Emission52 = ( saturate( min( blendOpSrc110 , blendOpDest110 ) ));
			o.Albedo = Emission52.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth80 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth80 = abs( ( screenDepth80 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamDistance ) );
			float4 clampResult87 = clamp( ( ( 1.0 - distanceDepth80 ) * _FoamPower * _FoamColor ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Foam88 = clampResult87;
			o.Emission = Foam88.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
-23;262;1374;723;486.4272;265.8996;1.6;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2233.852,-558.9497;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;102;-2419.711,-727.8766;Inherit;False;Constant;_Vector3;Vector 3;10;0;Create;True;0;0;False;0;False;0,0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;100;-2661.596,-846.7609;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-2352.337,-848.2412;Inherit;False;Property;_CloudSpeed;CloudSpeed;12;0;Create;True;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;25;-2196.461,-289.2936;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;24;-2161.151,-433.2819;Inherit;False;Constant;_Vector4;Vector 4;5;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;9;-2191.45,-201.5197;Inherit;False;Property;_WaveSpeed;WaveSpeed;2;0;Create;True;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;104;-2396.542,-1004.318;Inherit;False;Constant;_Vector5;Vector 5;10;0;Create;True;0;0;False;0;False;0.1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-2197.948,-742.082;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;18;-1986.297,-463.541;Inherit;False;Radial Shear;-1;;2;c6dc9fc7fa9b08c4d95138f2ae88b526;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-2184.704,-998.3662;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;92;-2205.233,54.12211;Inherit;False;1466.279;499.9053;;8;81;80;91;84;82;83;87;88;FOAM;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1920.361,-169.0268;Inherit;False;Property;_WaveScale;WaveScale;3;0;Create;True;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-2016.189,-290.4478;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;106;-2018.951,-919.6969;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;109;-1717.114,-897.249;Inherit;False;Property;_CloudScale;CloudScale;11;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;95;-1787.898,-801.458;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;93;-1786.188,-1050.382;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2155.233,213.9266;Inherit;False;Property;_FoamDistance;FoamDistance;8;0;Create;True;0;0;False;0;False;0;0.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;6;-1736.876,-415.6046;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;7.89;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;12;-1665.167,-153.9122;Inherit;False;Property;_WavePower;WavePower;4;0;Create;True;0;0;False;0;False;0.05;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;107;-1527.214,-1059.195;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-1489.582,-412.5443;Inherit;False;Property;_WaveColor;WaveColor;1;1;[HDR];Create;True;0;0;False;0;False;0.002709357,0.1886792,0,0;0.1782683,0.7735849,0.07662869,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;11;-1505.417,-224.774;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;108;-1529.734,-799.2034;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;80;-1932.722,107.5656;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;-1244.802,-562.664;Inherit;False;Property;_WaterColor;WaterColor;0;0;Create;True;0;0;False;0;False;0.06460939,0.1320755,0.01682094,0;0.2519046,0.5471698,0.1780883,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;84;-1844.681,219.1224;Inherit;False;Property;_FoamPower;FoamPower;9;0;Create;True;0;0;False;0;False;0;0.39;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;116;-1181.564,-798.3119;Inherit;False;Property;_CloudColor;CloudColor;10;0;Create;True;0;0;False;0;False;0.1527808,0.3490566,0.07079922,0;0.1970571,0.4245283,0.1702118,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;82;-1667.311,104.7477;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1198.17,-321.0562;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;97;-1240.859,-1059.323;Inherit;True;Multiply;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;91;-1741.08,342.0278;Inherit;False;Property;_FoamColor;FoamColor;7;0;Create;True;0;0;False;0;False;0.0602065,0.254902,0,0;0.3371313,0.4150943,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-886.3933,-931.0159;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;113;-963.0905,-379.4544;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1470.68,104.1221;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;110;-622.5105,-431.1262;Inherit;True;Darken;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;87;-1236.827,107.528;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;88;-962.9553,111.8873;Inherit;False;Foam;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;52;-316.513,-427.7192;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;78;-2216.373,642.6547;Inherit;False;2154.74;616.0648;Comment;12;77;30;32;31;29;37;47;35;34;36;44;38;MoveWater;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;34;-1224.227,987.7454;Inherit;True;Gradient;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;44;-791.6434,1044.353;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;79;356.9598,304.2707;Inherit;False;77;MoveVertexY;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1489.058,1142.719;Inherit;False;Property;_WaveAmplitude;WaveAmplitude;6;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-1554.547,967.7349;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-858.1284,788.1315;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-553.6702,692.6547;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;29;-1776.418,860.9056;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1956.653,939.7495;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2166.373,994.012;Inherit;False;Property;_MoveSpeed;MoveSpeed;5;0;Create;True;0;0;False;0;False;0.1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;77;-285.632,700.8207;Inherit;False;MoveVertexY;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;30;-2160.905,888.72;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;302.4631,-139.719;Inherit;False;52;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;298.7189,13.97546;Inherit;False;88;Foam;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;35;-1316.844,707.8049;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;586.4448,-47.64548;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;101;0;100;0
WireConnection;101;1;102;0
WireConnection;101;2;114;0
WireConnection;18;1;22;0
WireConnection;18;3;24;0
WireConnection;103;0;104;0
WireConnection;103;1;100;0
WireConnection;103;2;114;0
WireConnection;8;0;25;0
WireConnection;8;1;9;0
WireConnection;95;0;106;0
WireConnection;95;2;101;0
WireConnection;93;0;106;0
WireConnection;93;2;103;0
WireConnection;6;0;18;0
WireConnection;6;1;8;0
WireConnection;6;2;10;0
WireConnection;107;0;93;0
WireConnection;107;1;109;0
WireConnection;11;0;6;0
WireConnection;11;1;12;0
WireConnection;108;0;95;0
WireConnection;108;1;109;0
WireConnection;80;0;81;0
WireConnection;82;0;80;0
WireConnection;15;0;16;0
WireConnection;15;1;11;0
WireConnection;97;0;107;0
WireConnection;97;1;108;0
WireConnection;115;0;97;0
WireConnection;115;1;116;0
WireConnection;113;0;73;0
WireConnection;113;1;15;0
WireConnection;83;0;82;0
WireConnection;83;1;84;0
WireConnection;83;2;91;0
WireConnection;110;0;113;0
WireConnection;110;1;115;0
WireConnection;87;0;83;0
WireConnection;88;0;87;0
WireConnection;52;0;110;0
WireConnection;34;0;37;0
WireConnection;34;1;47;0
WireConnection;37;1;29;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;38;0;36;0
WireConnection;38;1;44;0
WireConnection;29;2;31;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;77;0;38;0
WireConnection;0;0;53;0
WireConnection;0;2;86;0
ASEEND*/
//CHKSM=2209A2515B82B6FA6D167ADD2696E49242C5DC7C