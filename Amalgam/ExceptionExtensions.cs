namespace Amalgam
{
	using System;

	internal static class ExceptionExtensions
	{
		public static T ThrowIfNull<T>(this T value, string paramName)
			where T : class
		{
			if (ReferenceEquals(null, value)) throw new ArgumentNullException(paramName);
			return value;
		}
	}
}