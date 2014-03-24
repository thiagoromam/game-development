texture rainbow;
sampler s0;
sampler rainbowSampler = sampler_state{ Texture = <rainbow>; AddressU = Clamp; AddressV = Clamp; };

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	float4 rainbow_color = tex2D(rainbowSampler, coords);

	if (color.a)
		return rainbow_color;

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}