shader_type canvas_item;

uniform vec2 center = vec2(0.5f, 0.5f);
uniform float radius = 0.3f;
uniform float innerRadius = 0.1f;
uniform float distortAmount = 0.5f;

void fragment() {
	float d = distance(UV, center);
	float h = clamp(d - innerRadius, 0, radius - innerRadius) / (radius - innerRadius);
	h = abs((h - 0.5) * 2.0);
	h = 1.0 - h;
	vec2 dirNormalized = normalize(UV - center);
	vec2 distortedUV = UV + dirNormalized * h * (radius - innerRadius) * distortAmount;
	vec4 color = texture(TEXTURE, distortedUV);
	color.a *= 1.5 - h;
	//color = vec4(distortedUV.xy, 0, 1);
	COLOR = color;
}
