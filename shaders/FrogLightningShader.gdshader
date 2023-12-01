shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;
uniform bool thunder = false;
uniform float duration=1;
uniform vec2 center ;
uniform float radius = 0.15;
uniform bool halfthunder = false;


void fragment() {
	
	
	vec4 bg = texture(SCREEN_TEXTURE, SCREEN_UV);

	if (length(SCREEN_UV - center) <= radius && !halfthunder){
		COLOR = vec4(0.06,0.05,0.15, length(SCREEN_UV - center)/radius *abs(duration));
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
