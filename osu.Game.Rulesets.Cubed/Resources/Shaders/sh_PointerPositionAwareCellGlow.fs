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
        highp vec2 resolution = v_TexRect.zw - v_TexRect.xy;

        // Normalized pixel coordinates (from 0 to 1)
        vec2 uv = (v_TexCoord - v_TexRect.xy) / resolution;
        vec2 mouse = (v_PointerPosition - v_TexRect.xy) / resolution;

        float border = 0.5 * step(min(min(uv.x, uv.y), min(1.0 - uv.x, 1.0 - uv.y)), borderThickness);

        vec4 glow = v_Colour * smoothstep(glowRadius, 0.0, distance(uv, mouse));
        vec4 borderEffect = v_Colour * border;

        fragColor = glow + borderEffect;
}

#endif
