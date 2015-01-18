Shader "DISPLACEMENT" {
   Properties {
   		_DISPVALUE ("DISP", range(-1,1)) = 0
   		_XPOS ("XPOS", float) = 0
   		_YPOS ("YPOS", float) = 0
   		_ZPOS ("ZPOS", float) = 0
   		_MOVE ("Move", float) = 0
   }
   SubShader {
   
      Pass {	
      	 Tags { "LightMode" = "ForwardBase" "Queue" = "Geometry" }

	     CGPROGRAM

	     #pragma vertex vert  
	     #pragma fragment frag 

		 #include "UnityCG.cginc"
		 #pragma target 3.0
 
 		uniform float4 _DiffuseColor;
 		uniform float4 _SpecularColor;
 		uniform float _Shininess;
 		uniform float _DISPVALUE;
 		uniform float _XPOS;
 		uniform float _YPOS;
 		uniform float _ZPOS;
 		uniform float _MOVE;
 		
 			
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            
         };
         struct vertexOutput {
			float4 pos : SV_POSITION;
			float4 normal : TEXCOORD0;
			float4 worldPos: TEXCOORD1;
			float4 posInObjectCoords : TEXCOORD2;
			float4 col : COLOR;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
            
            float4x4 modelMatrix = _Object2World;
            float4x4 viewMatrix = UNITY_MATRIX_V; 
            float4x4 projectionMatrix = UNITY_MATRIX_P;
            
            //Find the world position
            output.worldPos = mul(_Object2World,input.vertex);
            
            //Find the normals in world coordinates
            float4x4 modelMatrixInverse = _World2Object;
            output.normal = float4(normalize(mul(float4(input.normal,0),modelMatrixInverse).xyz),0);
            
            output.pos = mul(projectionMatrix,mul(viewMatrix,mul(modelMatrix,input.vertex)));
            
           // output.posInObjectCoords = input.vertex;
           
          // if(input.vertex.y > _DISPVALUE){ //THE BADASS ONE.
           
           if(input.vertex.y < _DISPVALUE){ // WAIT THIS ALSO JUST WORKS.
            	//discard; //Discards just doesn't render this fragment. Can be useful :)
            	
            	float4x4 change;
            	
            	if(_MOVE == 1){
            		float4x4 mover = float4x4(1,0,0,_XPOS,
	            						  	  0,1,0,_YPOS,
	            						  	  0,0,1,_ZPOS,
	            							  0,0,0,1);
            		
            		change = mul(UNITY_MATRIX_MVP, mover);			  
            	}
            	else if(_MOVE == 0){
            	
            	  float4x4 scaler = float4x4(_XPOS,0,0, 0,
										  	  0,_YPOS,0,0,
										  	  0,0,_ZPOS,0,
											  0,0,0,    1);
				  
				  change = mul(UNITY_MATRIX_MVP, scaler);
				  
            	}
            	else if(_MOVE == 2){
            	
            	  float4x4 rotater = float4x4(1,0,0, 				   0,
										  	  0,cos(_XPOS),-sin(_XPOS),0,
										  	  0,sin(_XPOS),cos(_XPOS), 0,
											  0,0,0,   				   1);
											  
				rotater = mul(rotater,float4x4 (cos(_YPOS),0,sin(_YPOS),0,
												0,1,0,0,
												-sin(_YPOS),0,cos(_YPOS),0,
												0,0,0,1));
												//IF you don't place the two 1's in this one, you get a very awesome result :D
            	
            	rotater = mul(rotater,float4x4 (cos(_ZPOS),-sin(_ZPOS),0,0,
            									sin(_ZPOS),cos(_ZPOS),0,0,
            									0,0,1,0,
            									0,0,0,1));
            	
            	change = mul(UNITY_MATRIX_MVP, rotater);
            	
            	}
            	else if(_MOVE == 3){
            	
            	  float4x4 scaler = float4x4(_XPOS*sin(_Time.y*5+10)+1.5,0,0, 0,
										  	  0,_YPOS*cos(_Time.y*5)+1.5,0,0,
										  	  0,0,_ZPOS*sin(_Time.y*30)+1.5,0,
											  0,0,0,1); //  && 
				  
				  change = mul(UNITY_MATRIX_MVP, scaler);
            	
            	}
            	
            	else{
            		change = UNITY_MATRIX_MVP;
            	}
            	
				 output.pos = mul(change,input.vertex);
            //	float4 ObjectInWorldSpace = float4(mul(modelMatrix,input.vertex));
            	
           // 	float4 disp = mul(_World2Object, float4(mul(ObjectInWorldSpace,mover)));
            	
            	
            	
            	
            //	output.pos = mul(clipMatrix,mover);
            }
           
           float offsets = cos(0.5*_Time.y)+0.4;
           
            output.col = input.vertex*sin(_Time.z)+offsets;
            
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
        //     float4 coloor = float4(1,1,0,1);
            return input.col;
           // return output;
            
         }
         ENDCG
      }
   }
}