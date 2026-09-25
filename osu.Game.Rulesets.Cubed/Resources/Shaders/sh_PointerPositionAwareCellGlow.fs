#ifndef POINTER_POSITION_AWARE_CELLGLOW_FS
#define POINTER_POSITION_AWARE_CELLGLOW_FS

#undef HIGH_PRECISION_VERTEX
#define HIGH_PRECISION_VERTEX

layout (location = 1) in lowp vec4 v_Colour;
layout (location = 2) in highp vec2 v_TexCoord;
layout (location = 3) in highp vec4 v_TexRect;
// Hack: use v_BlendRange to pass pointer position, using TexturedVertex2D efficiently
layout (location = 4) in mediump vec2 v_PointerPosition;

layout (location = 0) out vec4 fragColor;

const float borderThickness = .01;
const float glowRadius = .5;

void main(void)
{
    float glow = 1.0 - smoothstep(0.0, glowRadius, length(v_TexCoord - v_PointerPosition));

    vec2 distanceToCenter = abs(v_TexCoord - .5);
    float border = .5 * step(.5 - borderThickness, max(distanceToCenter.x, distanceToCenter.y));

    fragColor = (glow + border) * v_Colour;
}

#endif
