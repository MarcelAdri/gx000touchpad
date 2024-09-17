namespace gx000data;

/// <summary>
/// Represents a variable in the data exchange system.
/// </summary>
/// <typeparam name="T">The type of the variable.</typeparam>
public interface IVariable<T>
{
    /// <summary>
    /// Represents an interface for variables in the gx000data namespace.
    /// </summary>
    /// <typeparam name="T">The type of the variable.</typeparam>
    string VariableName { get; }

    /// <summary>
    /// Represents the status of a variable in data exchange.
    /// </summary>
    DataExchange.DataStatus Status { get; set; }

    /// <summary>
    /// Represents the value of a variable.
    /// </summary>
    /// <typeparam name="T">The type of the variable value.</typeparam>
    /// <returns>The current value of the variable.</returns>
    /// <seealso cref="IVariable{T}"/>
    /// <seealso cref="DataExchange.DataStatus"/>
    /// <example>
    /// The following example demonstrates how to use the Value property.
    /// <code>
    /// IVariable<see cref="int"/> variable = new MyVariable();
    /// variable.Value = 10;
    /// Console.WriteLine(variable.Value); // Output: 10
    /// </code>
    /// </example>
    T Value { get; set; }
}