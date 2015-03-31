sampler s0;

float4 PixelShaderFunction(float4 position : SV_POSITION, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 texColor = tex2D(s0, coords);

	if (!any(texColor)) return texColor;

	float step = 1.0 / 7;

	if (coords.x < (step * 1)) texColor = float4(1, 0, 0, 1);
	else if (coords.x < (step * 2)) texColor = float4(1, 0.5, 0, 1);
	else if (coords.x < (step * 3)) texColor = float4(1, 1, 0, 1);
	else if (coords.x < (step * 4)) texColor = float4(0, 1, 0, 1);
	else if (coords.x < (step * 5)) texColor = float4(0, 0, 1, 1);
	else if (coords.x < (step * 6)) texColor = float4(0.3, 0, 0.8, 1);
	else texColor = float4(1, 0.8, 1, 1);

	return texColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}