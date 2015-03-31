sampler s0;

float4 PixelShaderFunction(float4 position : SV_POSITION, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 texColor = tex2D(s0, coords);

	float high = .6;
	float low = .4;
		
	if (texColor.r > high) texColor.r = 1;
	else if (texColor.r < low) texColor.r = 0;

	if (texColor.g > high) texColor.g = 1;
	else if (texColor.g < low) texColor.g = 0;

	if (texColor.b > high) texColor.b = 1;
	else if (texColor.b < low) texColor.b = 0;

	return texColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}