sampler s0;

texture rainbow;
sampler rainbowSampler = sampler_state
{ 
	Texture = <rainbow>;
	AddressU = Clamp;
	AddressV = Clamp;
};

float4 PixelShaderFunction(float4 position : SV_POSITION, float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 texColor = tex2D(s0, coords);
	float4 rainbow_color = tex2D(rainbowSampler, coords);

	if (texColor.a)
		return rainbow_color;

	return texColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}