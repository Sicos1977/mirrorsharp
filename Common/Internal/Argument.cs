using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeStaticMemberQualifier
namespace MirrorSharp.Internal;

/// <summary>
///     Provides methods for verification of argument preconditions.
/// </summary>
internal static class Argument
{
    // ReSharper restore CheckNamespace

    private const string PotentialDoubleEnumeration = "Using NotNullOrEmpty with plain IEnumerable may cause double enumeration. Please use a collection instead.";

    /// <summary>
    ///     Verifies that a given argument value is not <c>null</c> and returns the value provided.
    /// </summary>
    /// <typeparam name="T">Type of the <paramref name="name" />.</typeparam>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <returns><paramref name="value" /> if it is not <c>null</c>.</returns>
    public static T NotNull<T>(string name, T value) where T : class?
    {
        return value ?? throw new ArgumentNullException(name);
    }

    /// <summary>
    ///     Verifies that a given argument value is not <c>null</c> and returns the value provided.
    /// </summary>
    /// <typeparam name="T">Type of the <paramref name="name" />.</typeparam>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <returns><paramref name="value" /> if it is not <c>null</c>.</returns>
    public static T NotNull<T>(string name, T? value) where T : struct
    {
        return value ?? throw new ArgumentNullException(name);
    }

    /// <summary>
    ///     Verifies that a given argument value is not <c>null</c> or empty and returns the value provided.
    /// </summary>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is empty.</exception>
    /// <returns><paramref name="value" /> if it is not <c>null</c> or empty.</returns>
    public static string NotNullOrEmpty(string name, string value)
    {
        NotNull(name, value);
        return value.Length == 0 ? throw NewArgumentEmptyException(name) : value;
    }

    /// <summary>
    ///     Verifies that a given argument value is not <c>null</c> or empty and returns the value provided.
    /// </summary>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is empty.</exception>
    /// <returns><paramref name="value" /> if it is not <c>null</c> or empty.</returns>
    public static T[] NotNullOrEmpty<T>(string name, T[] value)
    {
        NotNull(name, value);
        return value.Length == 0 ? throw NewArgumentEmptyException(name) : value;
    }

    /// <summary>
    ///     Verifies that a given argument value is not <c>null</c> or empty and returns the value provided.
    /// </summary>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is empty.</exception>
    /// <returns><paramref name="value" /> if it is not <c>null</c> or empty.</returns>
    public static TCollection NotNullOrEmpty<TCollection>(string name, TCollection value)
        where TCollection : class, IEnumerable {
        NotNull(name, value);
        var enumerator = value.GetEnumerator();
        try {
            if (!enumerator.MoveNext())
                throw NewArgumentEmptyException(name);
        }
        finally {
            (enumerator as IDisposable)?.Dispose();
        }

        return value;
    }

    /// <summary>
    ///     Verifies that a given <see cref="ReadOnlySpan{T}" /> argument value is not empty and returns the value provided.
    /// </summary>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value" /> is empty.</exception>
    /// <returns><paramref name="value" /> if it is not empty.</returns>
    public static ReadOnlySpan<T> NotEmpty<T>(string name, ReadOnlySpan<T> value) {
        return !value.IsEmpty ? value : throw NewArgumentEmptyException(name);
    }

    /// <summary>
    ///     (DO NOT USE) Ensures that NotNullOrEmpty can not be used with plain <see cref="IEnumerable" />,
    ///     as this may cause double enumeration.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(PotentialDoubleEnumeration, true)]
    // ReSharper disable UnusedParameter.Global
    public static void NotNullOrEmpty(string name, IEnumerable value) {
        // ReSharper restore UnusedParameter.Global
        throw new Exception(PotentialDoubleEnumeration);
    }

    /// <summary>
    ///     (DO NOT USE) Ensures that NotNullOrEmpty can not be used with plain <see cref="IEnumerable{T}" />,
    ///     as this may cause double enumeration.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(PotentialDoubleEnumeration, true)]
    // ReSharper disable UnusedParameter.Global
    public static void NotNullOrEmpty<T>(string name, IEnumerable<T> value) {
        // ReSharper restore UnusedParameter.Global
        throw new Exception(PotentialDoubleEnumeration);
    }

    private static Exception NewArgumentEmptyException(string name) {
        return new ArgumentException("Value can not be empty.", name);
    }

    /// <summary>
    ///     Casts a given argument into a given type if possible.
    /// </summary>
    /// <typeparam name="T">Type to cast <paramref name="value" /> into.</typeparam>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="value" /> can not be cast into type
    ///     <typeparamref name="T" />.
    /// </exception>
    /// <returns><paramref name="value" /> cast into <typeparamref name="T" />.</returns>
    public static T Cast<T>(string name, object value)
    {
        return value is not T value1 ? throw new ArgumentException($"The value \"{value}\" isn't of type \"{typeof(T)}\".", name) : value1;
    }

    /// <summary>
    ///     Verifies that a given argument is not null and casts it into a given type if possible.
    /// </summary>
    /// <typeparam name="T">Type to cast <paramref name="value" /> into.</typeparam>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="value" /> can not be cast into type
    ///     <typeparamref name="T" />.
    /// </exception>
    /// <returns><paramref name="value" /> cast into <typeparamref name="T" />.</returns>
    public static T NotNullAndCast<T>(string name, object value)
    {
        NotNull(name, value);
        return Cast<T>(name, value);
    }

    /// <summary>
    ///     Verifies that a given argument value is greater than or equal to zero and returns the value provided.
    /// </summary>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value" /> is less than zero.</exception>
    /// <returns><paramref name="value" /> if it is greater than or equal to zero.</returns>
    public static int PositiveOrZero(string name, int value)
    {
        return value < 0 ? throw new ArgumentOutOfRangeException(name, value, "Value must be positive or zero.") : value;
    }

    /// <summary>
    ///     Verifies that a given argument value is greater than zero and returns the value provided.
    /// </summary>
    /// <param name="name">Argument name.</param>
    /// <param name="value">Argument value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value" /> is less than or equal to zero.</exception>
    /// <returns><paramref name="value" /> if it is greater than zero.</returns>
    public static int PositiveNonZero(string name, int value)
    {
        return value <= 0 ? throw new ArgumentOutOfRangeException(name, value, "Value must be positive and not zero.") : value;
    }
}