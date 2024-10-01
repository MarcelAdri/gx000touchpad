// //     * Copyright (c) 2024 - 2024 Marcel Adriani
// //     *
// //     * This file is part of gx000touchpad.
// //
// //      * gx000touchpad is free software: you can redistribute it and/or modify it under the terms of the
// //          GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
// //          or (at your option) any later version.
// //
// //     * gx000touchpad is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// //          without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// //          See the GNU General Public License for more details.
// //
// //     * You should have received a copy of the GNU General Public License along with Foobar.
// //          If not, see <https://www.gnu.org/licenses/>.

using GeneralUtilities;

namespace gx000touchpadUnitTests;

[TestFixture]
public class RandomGeneratorTests
{
    private RandomGenerator _randomGenerator;

    [SetUp]
    public void SetUp()
    {
        _randomGenerator = new RandomGenerator();
    }
    
    [Test]
    public void RandomIntNumber_WhenCalled_ShouldReturnRandomNumberBetweenMinAndMax()
    {
        int min = 0;
        int max = 10;

        int result = _randomGenerator.RandomIntNumber(min, max);

        Assert.That(result, Is.GreaterThanOrEqualTo(min).And.LessThan(max));
    }

    [Test]
    public void RandomIntNumber_MinIsGreaterThanMax_ThrowArgumentOutOfRangeException()
    {
        int min = 10;
        int max = 0;
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _randomGenerator.RandomIntNumber(min, max));
    }
    
    [Test]
    public void RandomIntNumber_MinIsEqualToMax_ThrowArgumentOutOfRangeException()
    {
        int min = 10;
        int max = 10;
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _randomGenerator.RandomIntNumber(min, max));
    }
    
    [Test]
    public void GenerateRandomString_WhenCalled_ShouldReturnRandomStringWithSpecifiedSizeAndOffset()
    {
        int size = 5;
        char offset = 'A';

        string result = _randomGenerator.GenerateRandomString(size, offset);

        Assert.That(result.Length, Is.EqualTo(size));
        Assert.That(result, Does.Match($"^[{offset}-{(char)(offset + 26 - 1)}]+$"));
    }

    [Test]
    public void GenerateRandomString_OffsetTooLarge_ThrowArgumentOutOfRangeException()
    {
        int size = 5;
        char offset = (char)(Char.MaxValue - 26 + 1);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _randomGenerator.GenerateRandomString(size, offset));
    }
}