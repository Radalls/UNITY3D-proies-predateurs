using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     <author>Cyril Dubos</author>
///     Class <c>Utilities</c> gives some general useful methods for the application.
/// </summary>
public class Utilities
{
    /// <summary>
    ///     Method <c>GetRandomDouble</c> returns a random <c>double</c> between two other values.
    /// </summary>
    /// <param name="random">a <c>System.Random</c> object</param>
    /// <param name="minimum">the lower value</param>
    /// <param name="maximum">the higher value</param>
    /// <returns>the random double</returns>
    public static double GetRandomDouble(System.Random random, double minimum, double maximum)
    {
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    /// <summary>
    ///     Method <c>GetRandomVector3</c> returns a random <c>Vector3</c> between two other <c>Vector3</c>.
    /// </summary>
    /// <param name="random">a <c>System.Random</c> object</param>
    /// <param name="minimum">the lower <c>Vector3</c></param>
    /// <param name="maximum">the higher <c>Vector3</c></param>
    /// <returns>the random <c>Vector3</c></returns>
    public static Vector3 GetRandomVector3(System.Random random, Vector3 minimum, Vector3 maximum)
    {
        float x = (float)GetRandomDouble(random, minimum.x, maximum.x);
        float y = (float)GetRandomDouble(random, minimum.y, maximum.y);
        float z = (float)GetRandomDouble(random, minimum.z, maximum.z);

        return new Vector3(x, y, z);
    }

    /// <summary>
    ///     Method <c>GetRandomVector3</c> returns a random <c>Vector3</c> between two other homogeneous <c>Vector3</c>.
    /// </summary>
    /// <param name="random">a <c>System.Random</c> object</param>
    /// <param name="minimum">the value of lower homogeneous <c>Vector3</c></param>
    /// <param name="maximum">the value of higher homogeneous <c>Vector3</c></param>
    /// <returns>the random <c>Vector3</c></returns>
    public static Vector3 GetRandomVector3(System.Random random, int minimum, int maximum)
    {
        float value = random.Next(minimum, maximum);

        return new Vector3(value, value, value);
    }

    /// <summary>
    ///     Method <c>GetRandomQuaternion</c> returns a random <c>Quaternion</c>.
    /// </summary>
    /// <param name="random">a <c>System.Random</c> object</param>
    /// <param name="onX"><c>true</c> the Quaternion rotates on X-axis, <c>false</c> otherwise (equals to zero)</param>
    /// <param name="onY"><c>true</c> the Quaternion rotates on Y-axis, <c>false</c> otherwise (equals to zero)</param>
    /// <param name="onZ"><c>true</c> the Quaternion rotates on Z-axis, <c>false</c> otherwise (equals to zero)</param>
    /// <returns>the random <c>Vector3</c></returns>
    public static Quaternion GetRandomQuaternion(System.Random random, bool onX, bool onY, bool onZ) {
        int x = onX ? random.Next(0, 360) : 0;
        int y = onY ? random.Next(0, 360) : 0;
        int z = onZ ? random.Next(0, 360) : 0;

        return Quaternion.Euler(x, y, z);
    }
}
