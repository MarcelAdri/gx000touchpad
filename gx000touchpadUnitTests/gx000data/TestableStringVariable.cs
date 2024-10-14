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

using gx000data;

namespace gx000touchpadUnitTests.gx000data;

public class TestableStringVariable : StringVariable
{
    public TestableStringVariable(string variableName, DataExchange.DataStatus dataStatus, string dataValue)
        : base(variableName, dataStatus, dataValue)
    {
        
    }
    public bool OnStatusChangedCalled { get; private set; }

    protected override void OnStatusChanged()
    {
        OnStatusChangedCalled = true;
        base.OnStatusChanged();
    }
}