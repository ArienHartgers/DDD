using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DDD.SharedKernel.Identifiers
{
    public static class SolidCode
	{
		private const char PrefixSeparator = 'X';
		private const char DashReplacement = 'U';
		private const string BaseCodes = "0123456789abcdefghijklmnopqrstuvwxyzUVXYZ";
		private static readonly sbyte[] BaseCodeReverseLookup = CreateReverseLookup(BaseCodes);
		private static readonly int Base = BaseCodes.Length;
        private const int DigitBase = 10;
        private const int NormalBase = 36;
        private static Random _random = new Random();
        private static int _lastTensSec = -1;
        private static int _randomValue = 0;

		public static string ToSolidCode(this byte[] bytes)
		{
			var sb = new StringBuilder();
			sb.AppendAsSolidCode(bytes);
			return sb.ToString();
		}

		//public static string ToSolidCode(this Guid guid)
		//{
		//	var sb = new StringBuilder();
		//	sb.AppendAsSolidCode(guid.ToByteArray());
		//	return sb.ToString();
		//}

		public static string ToSolidCode(this Guid guid, Prefix prefix)
		{
			var sb = new StringBuilder();
			sb.AppendPrefix(prefix);
			sb.AppendAsSolidCode(guid.ToByteArray());
			return sb.ToString();
		}

		public static string ToSolidCode(this uint id, Prefix prefix)
		{
			var sb = new StringBuilder();
			sb.AppendPrefix(prefix);
			sb.AppendAsSolidCode(id);
			return sb.ToString();
		}

		public static string ToSolidCode(this int id, Prefix prefix)
		{
			if (id < 0)
			{
				throw new InvalidOperationException("Id cannot be negative");
			}

			return ToSolidCode((uint)id, prefix);
		}

		public static string ToSolidCode(this string id, Prefix prefix)
		{
			var sb = new StringBuilder();
			sb.AppendPrefix(prefix);
			sb.AppendAsSolidCode(id);
			return sb.ToString();
		}

		public static string NewSolidCode(Prefix prefix)
		{
			var sb = new StringBuilder();
			sb.AppendPrefix(prefix);
			sb.AppendNewSolidCode();
			return sb.ToString();
		}

		public static string AppendNewSolidCode(this StringBuilder sb)
		{
            var dt = DateTime.Now;
            var year = (dt.Year - 2000) % (DigitBase * NormalBase);
            var tensSec = dt.Minute * 600 + dt.Second * 10 + dt.Millisecond / 100;
            int rValue = GetRandomValue(tensSec);

			sb.Append(BaseCodes[(byte)(year / NormalBase)]);
            sb.Append(BaseCodes[(byte)(year % NormalBase)]);
            sb.Append(BaseCodes[(byte)dt.Month]);
            sb.Append(BaseCodes[(byte)dt.Day]);
            sb.Append(BaseCodes[(byte)dt.Hour]);
            sb.Append(BaseCodes[(byte)(tensSec / (NormalBase * NormalBase))]);
            sb.Append(BaseCodes[(byte)(tensSec / NormalBase) % NormalBase]);
            sb.Append(BaseCodes[(byte)(tensSec % NormalBase)]);

            for (int i = 0; i < 5; i++)
            {
                sb.Append(BaseCodes[rValue % NormalBase]);
                rValue /= NormalBase;
            }

            return sb.ToString();
		}

		public static void AppendAsSolidCode(this StringBuilder sb, byte[] bytes)
		{
			int maxIndex = bytes.Length - 1;
			int index = 0;

			while (index <= maxIndex)
			{
				var val = bytes[index++];

				sb.Append(BaseCodes[val >> 4]);
				sb.Append(BaseCodes[val & 0xF]);
			}
		}

		public static void AppendAsSolidCode(this StringBuilder sb, uint id)
		{
			bool start = false;
			AppendByte((byte)(id >> 24));
			AppendByte((byte)(id >> 16));
			AppendByte((byte)(id >> 8));
			AppendByte((byte)id);

			void AppendByte(byte val)
			{
				if (val > 0)
				{
					start = true;
				}

				if (start)
				{
					sb.Append(BaseCodes[val >> 4]);
					sb.Append(BaseCodes[val & 0xF]);
				}
			}
		}


		public static void AppendAsSolidCode(this StringBuilder sb, Guid guid)
		{
			sb.AppendAsSolidCode(guid.ToByteArray());
		}

		public static void AppendAsSolidCode(this StringBuilder sb, string id)
		{
			foreach (var c in id)
			{
				if (Char.IsDigit(c) || Char.IsLower(c))
				{
					sb.Append(c);
				}
				else if (c == '-')
				{
					sb.Append(DashReplacement);
				}
				else
				{
					throw new InvalidOperationException("Prefix may only contain lowercase characters");
				}
			}
		}

		internal static void AppendPrefix(this StringBuilder sb, Prefix prefix)
		{
			sb.Append(prefix.Value);
			sb.Append(PrefixSeparator);
		}

		public static bool TryParseBytes(string solidCode, [NotNullWhen(true)] out byte[] bytes)
		{
			return TryParseBytes(solidCode.AsSpan(), out bytes);
		}

		public static bool TryParseBytes(ReadOnlySpan<char> solidCode, [NotNullWhen(true)] out byte[] bytes)
		{
			int solidLength = solidCode.Length;
			int byteLen = solidLength / 2;
			var reqLen = byteLen * 2;
			if (solidCode.Length != reqLen)
			{
				bytes = null!;
				return false;
			}

			var result = new byte[byteLen];

			int solidIndex = 0;
			int byteIndex = 0;

			while (solidIndex < solidLength)
			{
				if (!getNextValue(solidCode, out var value))
				{
					bytes = null!;
					return false;
				}

				if (!getNextValue(solidCode, out var v2))
				{
					bytes = null!;
					return false;
				}

				value = value * Base + v2;

				result[byteIndex++] = (byte)value;
			}

			bytes = result;
			return true;

			bool getNextValue(ReadOnlySpan<char> solidCode, out int value)
			{
				var c = (byte)solidCode[solidIndex++];
				if (c > BaseCodeReverseLookup.Length)
				{
					value = 0;
					return false;
				}

				var v = BaseCodeReverseLookup[c];
				if (v < 0)
				{
					value = 0;
					return false;
				}

				value = v;
				return true;
			}
		}

		public static bool TryParseGuid(string solidCode, Prefix prefix, [NotNullWhen(true)] out Guid guid)
		{
			if (solidCode.StartsWith(prefix.Value + PrefixSeparator))
			{
				if (TryParseBytes(solidCode.AsSpan(prefix.Value.Length + 1), out var bytes))
				{
					if (bytes.Length == 16)
					{
						guid = new Guid(bytes);
						return true;
					}
				}
			}

			guid = Guid.Empty;
			return false;
		}


		public static bool TryParseUInt(string solidCode, Prefix prefix, [NotNullWhen(true)] out uint id)
		{
			if (solidCode.StartsWith(prefix.Value + PrefixSeparator))
			{
				if (TryParseBytes(solidCode.AsSpan(prefix.Value.Length + 1), out var bytes))
				{
					if (bytes.Length <= 4)
					{
						id = 0;
						foreach (var b in bytes)
						{
							id = (id << 8) | b;
						}

						return true;
					}
				}
			}

			id = 0;
			return false;
		}

		private static sbyte[] CreateReverseLookup(string baseCodes)
		{
			var baseCodeLookup = new sbyte[128];

			Array.Fill<sbyte>(baseCodeLookup, -1);
			sbyte val = 0;
			foreach (char c in baseCodes)
			{
				baseCodeLookup[(byte)c] = val++;
			}

			return baseCodeLookup;
		}

        private static int GetRandomValue(int tensSec)
        {
            lock (_random)
            {
                if (_lastTensSec != tensSec)
                {
                    _lastTensSec = tensSec;
                    _randomValue = _random.Next();
                }
                else
                {
                    _randomValue++;
                }

                return _randomValue;
            }
        }
	}
}