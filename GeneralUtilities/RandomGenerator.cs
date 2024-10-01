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
    /// Generates a random string of the specified size using the given offset.
    /// </summary>
    /// <param name="size">The size of the random string to generate.</param>
    /// <param name="offset">The starting character offset for generating the random string.</param>
    /// <returns>A string containing the randomly generated characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when offset + MaxLettersOffset are more than Char.MaxValue</exception>
    public string GenerateRandomString(int size, char offset)
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
}
