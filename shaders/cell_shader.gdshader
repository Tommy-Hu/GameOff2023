shader_type canvas_item;
uniform vec2 offset;
uniform vec4 tint;

// copied from https://godotshaders.com/snippet/2d-noise/
vec2 random(vec2 uv){
    uv = vec2( dot(uv, vec2(127.1,311.7) ),
               dot(uv, vec2(269.5,183.3) ) );
    return -1.0 + 2.0 * fract(sin(uv) * 43758.5453123);
}

float noise(vec2 uv) {
    vec2 uv_index = floor(uv);
    vec2 uv_fract = fract(uv);

    vec2 blur = smoothstep(0.0, 1.0, uv_fract);

    return mix( mix( dot( random(uv_index + vec2(0.0,0.0) ), uv_fract - vec2(0.0,0.0) ),
                     dot( random(uv_index + vec2(1.0,0.0) ), uv_fract - vec2(1.0,0.0) ), blur.x),
                mix( dot( random(uv_index + vec2(0.0,1.0) ), uv_fract - vec2(0.0,1.0) ),
                     dot( random(uv_index + vec2(1.0,1.0) ), uv_fract - vec2(1.0,1.0) ), blur.x), blur.y) + 0.5;
}


void vertex() {
  VERTEX += vec2(cos(TIME)*10.0, sin(TIME)*10.0);
}
float map(float value, float min1, float max1, float min2, float max2) {
  return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}
void fragment() {
	float n = mix(0.45, 0.55, clamp(noise(UV * 1.1f + vec2((sin(TIME) + offset.r), (cos(TIME)))), 0, 1));
	// here n is between 0.45 and 0.55
	// UV is between 0 and 1
	vec2 modifiedUV = (UV - 0.5) * 2.0;
	modifiedUV *= map(n, 0.45, 0.55, 1, 1.25);
	modifiedUV = modifiedUV * 0.5 + 0.5;
	vec3 color = texture(TEXTURE, modifiedUV).rgb;
	vec3 colorInv = ceil(vec3(1, 1, 1) - color);
	float a = 1.0 - color.r;
	float clamppedA = clamp(a * 3.0 * (n), 0, 1);
	a = pow(clamppedA, 2);
	float c = round(a);
	if (c == 0.0) {
		
		// this pixel is outside the core
		if (ceil(a) == 1.0) {
			// this pixel is inside the bounds of the membrane
			a = (1.0f - clamppedA) * 0.2f;
		}
	}
	COLOR = (tint + vec4(0.5, 0.5, 0.5, 1)) * vec4(colorInv, a);
}
