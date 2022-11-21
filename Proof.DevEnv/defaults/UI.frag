#version 330 core

uniform sampler2D[8] Textures;

in vec2 _texCoords;
flat in int _texSlot;

out vec4 colour;

void main() {
	// Update this to a preprocessor function or something because this is fucking stupid
	if(_texSlot == 0) {
		colour = texture(Textures[0], _texCoords);
	} else if(_texSlot == 1) {
		colour = texture(Textures[1], _texCoords);
	} else if(_texSlot == 2) {
		colour = texture(Textures[2], _texCoords);
	} else if(_texSlot == 3) {
		colour = texture(Textures[3], _texCoords);
	} else if(_texSlot == 4) {
		colour = texture(Textures[4], _texCoords);
	} else if(_texSlot == 5) {
		colour = texture(Textures[5], _texCoords);
	} else if(_texSlot == 6) {
		colour = texture(Textures[6], _texCoords);
	} else if(_texSlot == 7) {
		colour = texture(Textures[7], _texCoords);
	} else {
		colour = vec4(0.7, 0.4, 0.5, 1.0);
	}
}