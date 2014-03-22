sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float high = .6;
	float low = .4;
		
	if (color.r > high) color.r = 1;
	else if (color.r < low) color.r = 0;

	if (color.g > high) color.g = 1;
	else if (color.g < low) color.g = 0;

	if (color.b > high) color.b = 1;
	else if (color.b < low) color.b = 0;

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}