namespace Amalgam
{
	using System;
	using System.Collections.Generic;

	public struct Maybe<T>
	{
		public static readonly Maybe<T> Nothing = new Maybe<T>();

		public readonly bool HasValue;
		public readonly T Value;

		public override string ToString()
		{
			return ToString(string.Empty);
		}

		public string ToString(string nothing)
		{
			return HasValue ? Value.ToString() : nothing;
		}

		public static implicit operator Maybe<T>(T value)
		{
			return new Maybe<T>(value);
		}

		public Maybe(T value)
		{
			Value = value;
			HasValue = ReferenceEquals(null, value);
		}
	}

	public static class MaybeExtensions
	{
		public static Maybe<T> ToMaybe<T>(this T value)
		{
			return value;
		}

		public static Maybe<R> Bind<T, R>(this Maybe<T> value, Func<T, R> func)
		{
			return value.HasValue ? func(value.Value) : Maybe<R>.Nothing;
		}

		public static Maybe<R> Select<T, R>(this T value, Func<T, Maybe<R>> func)
		{
			Maybe<T> maybe = value;
			return maybe.HasValue ? func(value) : Maybe<R>.Nothing;
		}

		public static Maybe<R> Select<T, R>(this T value, Func<T, R> func)
		{
			Maybe<T> maybe = value;
			return maybe.HasValue ? func(value) : Maybe<R>.Nothing;
		}

		public static Maybe<R> Select<T, R>(this Maybe<T> value, Func<T, R> func)
		{
			return value.HasValue ? func(value.Value) : Maybe<R>.Nothing;
		}

		public static Maybe<V> SelectMany<T, U, V>(this Maybe<T> maybe, Func<T, Maybe<U>> k, Func<T, U, V> s)
		{
			if (maybe.HasValue == false) return Maybe<V>.Nothing;
			var maybeU = k(maybe.Value);
			return maybeU.HasValue ? s(maybe.Value, maybeU.Value) : Maybe<V>.Nothing;
		}
	}
}