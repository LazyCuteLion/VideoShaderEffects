
sampler2D input : register(s0);

float rows : register(c0) = float(1.0);
float columns : register(c1) = float(1.0);
float2 spacing : register(c2);
float4 spacingColor : register(c4);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float h = spacing.x * (columns - 1);
    float v = spacing.y * (rows - 1);

    float cellWidth = (1.0 - h) / columns;
    float cellHeight = (1.0 - v) / rows;

    float colIndex = floor(uv.x / (cellWidth + spacing.x));
    float rowIndex = floor(uv.y / (cellHeight + spacing.y));

    float sx = colIndex * (cellWidth + spacing.x);
    float sy = rowIndex * (cellHeight + spacing.y);

    float2 localUV = float2((uv.x - sx) / cellWidth, (uv.y - sy) / cellHeight);

    if (uv.x - sx >= cellWidth || uv.y - sy >= cellHeight)
    {
        float4 color = float4(spacingColor.rgba);
        color.rgb *= color.a;
        return color;
    }

    return tex2D(input, localUV);
}
