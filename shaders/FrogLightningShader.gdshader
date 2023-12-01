shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;
uniform bool thunder = false;
uniform float duration;
uniform vec2 center ;
uniform float radius = 0.1;


void fragment() {
	

	vec4 bg = texture(SCREEN_TEXTURE, SCREEN_UV);
	if (length(SCREEN_UV - center) <= radius){
		
	}else{
	COLOR = vec4(0.06,0.05,0.15,1);
	if (thunder){
		if (duration >= 0.0){
			COLOR = vec4(0.8,0.8,0.8, duration);
		}
		if (duration <= 0.0){
			COLOR = vec4(0.06,0.05,0.15, abs(duration));
		}
		
	}
	}

}
