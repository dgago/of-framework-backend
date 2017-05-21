using System;
using System.Collections;

namespace of
{
	public static class ValidationExtensions
	{
		public static void NotNull(this object arg, string name)
		{
			if (arg == null)
			{
				throw new ArgumentException($"El valor de {name} debe ser especificado.");
			}
		}

		public static void NotNull(this string arg, string name)
		{
			if (string.IsNullOrWhiteSpace(arg))
			{
				throw new ArgumentException($"El valor de {name} debe ser especificado.");
			}
		}

		public static void NotEmpty(this IList arg, string name)
		{
			if (arg == null)
			{
				throw new ArgumentException($"El valor de {name} debe ser especificado.");
			}

			if (arg.Count == 0)
			{
				throw new ArgumentException($"La lista {name} no puede estar vacía.");
			}
		}
	}
}