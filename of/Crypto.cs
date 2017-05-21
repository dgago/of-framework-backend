using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace of
{
	internal static class Base64Url
	{
		public static string Encode(byte[] arg)
		{
			return Convert.ToBase64String(arg).Split('=')[0].Replace('+', '-').Replace('/', '_');
		}

		public static byte[] Decode(string arg)
		{
			string s = arg.Replace('-', '+').Replace('_', '/');
			switch (s.Length % 4)
			{
			case 0:
				return Convert.FromBase64String(s);
			case 2:
				s += "==";
				goto case 0;
			case 3:
				s += "=";
				goto case 0;
			default:
				throw new Exception("Illegal base64url string!");
			}
		}
	}

	public class CryptoRandom : Random
	{
		private readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
		private readonly byte[] _uint32Buffer = new byte[4];

		public CryptoRandom()
		{
		}

		public CryptoRandom(int ignoredSeed)
		{
		}

		public static byte[] CreateRandomKey(int length)
		{
			byte[] data = new byte[length];
			new RNGCryptoServiceProvider().GetBytes(data);
			return data;
		}

		public static string CreateRandomKeyString(int length)
		{
			return Base64Url.Encode(CreateRandomKey(length));
		}

		public static string CreateUniqueId(int length = 16)
		{
			byte[] numArray = new byte[length];
			new RNGCryptoServiceProvider().GetBytes(numArray);
			return ByteArrayToString(numArray);
		}

		private static string ByteArrayToString(byte[] ba)
		{
			StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
			foreach (byte num in ba)
			{
				stringBuilder.AppendFormat("{0:x2}", num);
			}
			return stringBuilder.ToString();
		}

		public override int Next()
		{
			_rng.GetBytes(_uint32Buffer);
			return BitConverter.ToInt32(_uint32Buffer, 0) & int.MaxValue;
		}

		public override int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxValue));
			}
			return Next(0, maxValue);
		}

		public override int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(minValue));
			}
			if (minValue == maxValue)
			{
				return minValue;
			}
			long num1 = maxValue - minValue;
			uint num2;
			long num3;
			long num4;
			do
			{
				_rng.GetBytes(_uint32Buffer);
				num2 = BitConverter.ToUInt32(_uint32Buffer, 0);
				num3 = 4294967296L;
				num4 = num3 % num1;
			} while (num2 >= num3 - num4);
			return (int) (minValue + num2 % num1);
		}

		public override double NextDouble()
		{
			_rng.GetBytes(_uint32Buffer);
			return BitConverter.ToUInt32(_uint32Buffer, 0) / 4294967296.0;
		}

		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer));
			}
			_rng.GetBytes(buffer);
		}
	}

	public static class Crypto
	{
		private const int Pbkdf2IterCount = 1000;
		private const int Pbkdf2SubkeyLength = 32;
		private const int SaltSize = 16;

		public static string HashPassword(string password)
		{
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}
			byte[] salt;
			byte[] bytes;
			using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 1000))
			{
				salt = rfc2898DeriveBytes.Salt;
				bytes = rfc2898DeriveBytes.GetBytes(32);
			}
			byte[] inArray = new byte[49];
			Buffer.BlockCopy(salt, 0, inArray, 1, 16);
			Buffer.BlockCopy(bytes, 0, inArray, 17, 32);
			return Convert.ToBase64String(inArray);
		}

		public static bool VerifyHashedPassword(string hashedPassword, string password)
		{
			if (hashedPassword == null)
			{
				return false;
			}
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}
			byte[] numArray = Convert.FromBase64String(hashedPassword);
			if (numArray.Length != 49 || numArray[0] != 0)
			{
				return false;
			}
			byte[] salt = new byte[16];
			Buffer.BlockCopy(numArray, 1, salt, 0, 16);
			byte[] a = new byte[32];
			Buffer.BlockCopy(numArray, 17, a, 0, 32);
			byte[] bytes;
			using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 1000))
			{
				bytes = rfc2898DeriveBytes.GetBytes(32);
			}
			return ByteArraysEqual(a, bytes);
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static bool ByteArraysEqual(byte[] a, byte[] b)
		{
			if (ReferenceEquals(a, b))
			{
				return true;
			}
			if (a == null || b == null || a.Length != b.Length)
			{
				return false;
			}
			bool flag = true;
			for (int index = 0; index < a.Length; ++index)
			{
				flag &= a[index] == b[index];
			}
			return flag;
		}
	}
}