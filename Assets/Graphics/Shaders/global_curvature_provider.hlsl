#ifndef GLOBAL_CURVATURE_PROVIDER_HLSL
#define GLOBAL_CURVATURE_PROVIDER_HLSL

float LTG8_CURVATURE;

void GetCurvature_float(out float curvature)
{
    curvature = LTG8_CURVATURE;
}

void GetCurvature_half(out float curvature)
{
    curvature = LTG8_CURVATURE;
}

#endif