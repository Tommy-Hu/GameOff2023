shader_type canvas_item;

uniform vec4 tint;

void fragment() {
	vec4 texColor = texture(TEXTURE, UV);
	vec3 color = texColor.rgb;
	color = 1.0 - color;
	COLOR = tint * vec4(color, 1.0 - texColor.a);
}
