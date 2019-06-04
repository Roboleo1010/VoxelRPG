#version 330
 
in vec3 vPosition;
in vec3 vColor;
in vec3 vNormal;

out vec4 color;

uniform mat4 modelview;
 
void main()
{
    gl_Position = modelview * vec4(vPosition, 1.0);
    color = vec4(vColor, 1);
}