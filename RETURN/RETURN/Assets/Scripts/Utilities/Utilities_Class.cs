using UnityEngine;
using System.Collections;

    public static class Utilities_Class
    {
        public static float normal(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        public static float lerp(float value, float min, float max)
        {
            return ((max - min) * value) + min;
        }

        public static float U_map(float value, float sourceMin, float sourceMax, float destMin, float destMax)
        {
            return lerp(normal(value, sourceMin, sourceMax), destMin, destMax);
        }

        public static float clamp(float value, float min, float max)
        {
            return Mathf.Min(Mathf.Max(value, Mathf.Min(min, max)), Mathf.Max(min, max));
        }

        public static float randomRange(float min, float max)
        {
            return min + Random.value * (max - min);
        }

        public static int randomInt(int min, int max)
        {
            return Mathf.FloorToInt(min + Random.value * (max - min + 1));
        }

        public static int randomDistribute(int min, int max, int iterations)
        {
            int total = 0;

            for (int i = 0; i < iterations; i++)
            {
                total += (int)randomRange(min, max);
            }

            return total / iterations;
        }

        public static float distance3D(Vector3 p0, Vector3 p1)
        {
            float dx = p1.x - p0.x;
            float dy = p1.y - p0.y;
            float dz = p1.z - p0.z;

            return Mathf.Sqrt((dx * dx + dy * dy) + dz * dz);
        }

        public static float distance2D(float x0, float x1, float y0, float y1)
        {
            float dx = x1 - x0;
            float dy = y1 - y0;

            return Mathf.Sqrt((dx * dx + dy * dy));
        }

        public static bool circleCollision(Vector3 targVec, Vector3 circVec, float rad)
        {
            return distance3D(targVec, circVec) < rad;
        }

        public static Vector3 Centre3D(Vector3 p0, Vector3 p1)
        {
            float dx = (p1.x + p0.x)/2;
            float dy = (p1.y + p0.y)/2;
            float dz = (p1.z + p0.z)/2;

            return new Vector3(dx, dy, dz);
        }

        public static Vector3 Rotate3D(Vector3 input, float a_x, float a_y, float a_z)
    {

        ////Get origin coords
        float o_x = input.x;
        float o_y = input.y;
        float o_z = input.z;

        a_x = (a_x * Mathf.Deg2Rad);
        a_y = (a_y * Mathf.Deg2Rad);
        a_z = (a_z * Mathf.Deg2Rad);

        float inputAngleZ = Mathf.Atan2(o_y, o_x);
        float inputAngleY = Mathf.Atan2(o_x, o_z);
        float inputAngleX = Mathf.Atan2(o_y, o_z);

        float CosZ = Mathf.Cos(a_z + inputAngleZ);
        float SinZ = Mathf.Sin(a_z + inputAngleZ);

        float CosX = Mathf.Cos(a_x + inputAngleX);
        float SinX = Mathf.Sin(a_x + inputAngleX);

        float CosY = Mathf.Cos(a_y + inputAngleY);
        float SinY = Mathf.Sin(a_y + inputAngleY);

        Vector3 R1 = new Vector3(CosZ * CosY, (CosZ * SinY * SinX) - (SinZ * CosX), (CosZ * SinY * CosX) + (SinZ * SinX));
        Vector3 R2 = new Vector3(SinZ * CosY, (SinZ * SinY * SinX) + (CosZ * CosX), (SinZ * SinY * CosX) - (CosZ * SinX));
        Vector3 R3 = new Vector3(-SinY, CosY * SinX, CosY * CosX);

        float x = R1.x + R1.y + R1.z;
        float y = R2.x + R2.y + R2.z;
        float z = R3.x + R3.y + R3.z;

        Vector3 outputVector = new Vector3(x, y, z);
        return outputVector;
    }
}
