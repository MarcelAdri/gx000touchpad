// /*
//     * Copyright (c) 2024 - 2024 Marcel Adriani
//     *
//     * This file is part of gx000touchpad.
// 
//      * gx000touchpad is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
//     * gx000touchpad is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// 
//     * You should have received a copy of the GNU General Public License along with Foobar. If not, see <https://www.gnu.org/licenses/>.
// 
//     */

using System.Text;

namespace GeneralUtilities;

/// <summary>
/// Represents a class that generates random numbers and strings.
/// </summary>
public class RandomGenerator
{
    private readonly object _lock = new();

    /// <summary>
    /// Represents a random number generator.
    /// </summary>
    private readonly Random _random = new Random();

    /// <summary>
    /// The length of the generated random string in lower case.
    /// </summary>
    private const int LowerCaseRandomStringLength = 4;

    /// <summary>
    /// The minimum value for generating random integer numbers.
    /// </summary>
    private const int RandomIntNumberMin = 1000;

    /// <summary>
    /// The maximum value for generating random integer numbers.
    /// </summary>
    private const int RandomIntNumberMax = 9999;

    /// <summary>
    /// The constant that defines the length of the upper case string in the random password.
    /// </summary>
    private const int UpperCaseRandomStringLength = 2;

    /// <summary>
    /// The maximum letters offset used for generating random strings.
    /// </summary>
    private const int MaxLettersOffset = 26;

    /// <summary>
    /// Generates a random integer number between the specified minimum and maximum values (exclusive).
    /// </summary>
    /// <param name="min">The minimum value (exclusive).</param>
    /// <param name="max">The maximum value (exclusive).</param>
    /// <returns>A random integer number between the specified minimum and maximum values (exclusive).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when min is not less than max.</exception>
    public int RandomIntNumber(int min, int max)
    {
        if (min >= max)
        {
            throw new ArgumentOutOfRangeException(nameof(min), "max has to be greater than min.");
        }

        lock (_lock)
        {
            return _random.Next(min, max);    
        }
        
    }

    /// <summary>
    /// Generates a random lowercase string of the specified size.
    /// </summary>
    /// <param name="size">The size of the random string to generate.</param>
    /// <returns>A random lowercase string of the specified size.</returns>
    public string RandomLowerCaseString(int size)
    {
        return GenerateRandomString(size, 'a');
    }

    /// <summary>
    /// Generates a random string consisting of uppercase letters.
    /// </summary>
    /// <param name="size">The length of the generated string.</param>
    /// <returns>A string consisting of uppercase letters.</returns>
    public string RandomUpperCaseString(int size)
    {
        return GenerateRandomString(size, 'A');
    }

    /// <summary>
    /// Generates a random string of the specified size using the given offset.
    /// </summary>
    /// <param name="size">The size of the random string to generate.</param>
    /// <param name="offset">The starting character offset for generating the random string.</param>
    /// <returns>A string containing the randomly generated characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when offset + MaxLettersOffset are more than Char.MaxValue</exception>
    private string GenerateRandomString(int size, char offset)
    {
        if (offset > Char.MaxValue - MaxLettersOffset)
        {
            throw new ArgumentOutOfRangeException(nameof(offset),
                $"offset must be between 0 and {Char.MaxValue - MaxLettersOffset}.");
        }
        var builder = new StringBuilder(size);
        
        for (var i = 0; i < size; i++)
        {
            char @char;
            lock (_lock)
            {
                @char = (char)_random.Next(offset, offset + MaxLettersOffset);
            }
            builder.Append(@char);
        }   
        return builder.ToString();
    }

    /// <summary>
    /// Generates a random password consisting of lowercase letters, numbers, and uppercase letters.
    /// </summary>
    /// <returns>A randomly generated password.</returns>
    public string RandomPassword()
    {
        var passwordBuilder = new StringBuilder();
        lock (_lock)
        {
            passwordBuilder.Append(RandomLowerCaseString(LowerCaseRandomStringLength))
                .Append(RandomIntNumber(RandomIntNumberMin, RandomIntNumberMax))
                .Append(RandomUpperCaseString(UpperCaseRandomStringLength));
        }
        
        return passwordBuilder.ToString();
    }  
}
