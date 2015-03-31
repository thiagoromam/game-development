sampler s0;

float4 PixelShaderFunction(float4 position : SV_POSITION, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 texColor = tex2D(s0, coords);
	if (texColor.a)
		texColor.rgb = 1 - texColor.rgb;

	return texColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}