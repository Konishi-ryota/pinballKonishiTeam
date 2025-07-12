// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:544,x:33242,y:32687,varname:node_544,prsc:2|emission-2678-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:5901,x:32141,y:32594,ptovrint:False,ptlb:main tecture,ptin:_maintecture,varname:node_5901,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:63,x:32346,y:32569,varname:node_63,prsc:2,ntxv:0,isnm:False|TEX-5901-TEX;n:type:ShaderForge.SFN_VertexColor,id:5019,x:32302,y:32765,varname:node_5019,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:1971,x:32297,y:33160,ptovrint:False,ptlb:FresnelValue,ptin:_FresnelValue,varname:node_1971,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Fresnel,id:3183,x:32297,y:33003,varname:node_3183,prsc:2|EXP-1971-OUT;n:type:ShaderForge.SFN_Power,id:6771,x:32465,y:33003,varname:node_6771,prsc:2|VAL-3183-OUT,EXP-619-OUT;n:type:ShaderForge.SFN_ValueProperty,id:619,x:32297,y:32937,ptovrint:False,ptlb:FrenelPower,ptin:_FrenelPower,varname:node_619,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:2678,x:32874,y:32673,varname:node_2678,prsc:2|A-7019-RGB,B-173-OUT;n:type:ShaderForge.SFN_Color,id:7019,x:32501,y:32569,ptovrint:False,ptlb:color,ptin:_color,varname:node_7019,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:8220,x:32643,y:33003,varname:node_8220,prsc:2|A-5019-RGB,B-6771-OUT;n:type:ShaderForge.SFN_Multiply,id:173,x:32861,y:33003,varname:node_173,prsc:2|A-8220-OUT,B-8780-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8780,x:32659,y:33208,ptovrint:False,ptlb:FresnelIntensity,ptin:_FresnelIntensity,varname:node_8780,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.5;proporder:5901-1971-619-7019-8780;pass:END;sub:END;*/

Shader "Custom/Opaquefresnel" {
    Properties {
        _maintecture ("main tecture", 2D) = "white" {}
        _FresnelValue ("FresnelValue", Float ) = 1
        _FrenelPower ("FrenelPower", Float ) = 1
        _color ("color", Color) = (0,0,0,1)
        _FresnelIntensity ("FresnelIntensity", Float ) = 1.5
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _FresnelValue)
                UNITY_DEFINE_INSTANCED_PROP( float, _FrenelPower)
                UNITY_DEFINE_INSTANCED_PROP( float4, _color)
                UNITY_DEFINE_INSTANCED_PROP( float, _FresnelIntensity)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _color );
                float _FresnelValue_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FresnelValue );
                float _FrenelPower_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FrenelPower );
                float _FresnelIntensity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FresnelIntensity );
                float3 emissive = (_color_var.rgb+((i.vertexColor.rgb*pow(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelValue_var),_FrenelPower_var))*_FresnelIntensity_var));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
