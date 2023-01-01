#define UNITY_PI            3.14159265359f
#define UNITY_TWO_PI        6.28318530718f

// Receives pos in 3D cartesian coordinates (x, y, z)
// Returns UV coordinates corresponding to pos using spherical texture mapping
void get_spherical_uv_float(const in float3 pos, out float2 uv)
{
    const float radius = length(pos);
    const float theta = atan2(pos.z, pos.x) / UNITY_TWO_PI;
    const float phi = acos(pos.y / radius);
    const float x_a = theta + 0.5;
    const float x_b = frac(theta + 1.0) - 0.5;
    uv = float2(fwidth(x_a) < fwidth(x_b) ? x_a : x_b, 1 - phi / UNITY_PI);
}

void get_spherical_uv_normalized_float(const in float3 pos, out float2 uv)
{
    const float3 pos_n = normalize(pos);
    const float theta = atan2(pos_n.z, pos_n.x) / UNITY_TWO_PI;
    const float phi = acos(pos_n.y);
    const float x_a = theta + 0.5;
    const float x_b = frac(theta + 1.0) - 0.5;
    uv = float2(fwidth(x_a) < fwidth(x_b) ? x_a : x_b, 1 - phi / UNITY_PI);
}

void get_spherical_uv_regular_float(const in float3 pos, out float2 uv)
{
    const float radius = length(pos);
    const float theta = atan2(pos.z, pos.x) / UNITY_TWO_PI;
    const float phi = acos(pos.y / radius);
    uv = float2(theta + 0.5, 1.0 - phi / UNITY_PI);
}
