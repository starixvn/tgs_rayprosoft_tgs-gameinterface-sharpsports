using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Tgs.GameInterface.SharpSports.Core;

internal sealed class ParameterEncryptor : IParameterEncryptor
{
	public string Encrypt(JsonObject values, string secret)
	{
		var keyBytes = Encoding.UTF8.GetBytes(secret);

		keyBytes = SHA1.HashData(keyBytes);
		keyBytes = SHA1.HashData(keyBytes);

		var keyFinal = new byte[16];
		Array.Copy(keyBytes, keyFinal, 16);

		var dataBytes = JsonSerializer.SerializeToUtf8Bytes(values);

		using var aes = Aes.Create();
		aes.Key = keyFinal;

		var encryptedBytes = aes.EncryptEcb(dataBytes, PaddingMode.PKCS7);
		var encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", "");
		var result = encryptedHex.ToUpper();

		return result;
	}
}
