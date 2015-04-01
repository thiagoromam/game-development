uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float4 PixelShaderFunction(float4 position : SV_POSITION, float4 color : COLOR0, float2 curCoord : TEXCOORD0) : COLOR0
{
	float2 center = {0.5f, 0.5f};
	float maxDistSQR = 0.7071f; //precalulated sqrt(0.5f)

	float2 diff = abs(curCoord - center);
	float distSQR = length(diff);
											
	float blurAmount = (distSQR / maxDistSQR) / 100.0f;

	float2 prevCoord = curCoord;
	prevCoord[0] -= blurAmount;

	float2 nextCoord = curCoord;
	nextCoord[0] += blurAmount;

	float4 texColor = ((tex2D(ScreenS, curCoord)
				  + tex2D(ScreenS, prevCoord)
				  + tex2D(ScreenS, nextCoord))/3.0f);
		
	return texColor;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}
