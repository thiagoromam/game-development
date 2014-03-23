sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	if (!any(color)) return color;

	float step = 1.0 / 7;

	if (coords.x < (step * 1)) color = float4(1, 0, 0, 1);
	else if (coords.x < (step * 2)) color = float4(1, 0.5, 0, 1);
	else if (coords.x < (step * 3)) color = float4(1, 1, 0, 1);
	else if (coords.x < (step * 4)) color = float4(0, 1, 0, 1);
	else if (coords.x < (step * 5)) color = float4(0, 0, 1, 1);
	else if (coords.x < (step * 6)) color = float4(0.3, 0, 0.8, 1);
	else color = float4(1, 0.8, 1, 1);

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}