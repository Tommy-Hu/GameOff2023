shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;
uniform bool thunder = false;
uniform float duration;


void fragment() {
	vec4 bg = texture(SCREEN_TEXTURE, SCREEN_UV);
	COLOR = vec4(0,0,0,1);
	if (thunder){
		if (duration >= 0.0){
			COLOR = vec4(255,255,255, duration);
		}
		if (duration <= 0.0){
			COLOR = vec4(0,0,0, abs(duration));
		}
		
	}

}
