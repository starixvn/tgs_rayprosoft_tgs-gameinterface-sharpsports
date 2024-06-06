using System.Text.Json.Nodes;

namespace Tgs.GameInterface.SharpSports.Core;

public interface IParameterEncryptor
{
	string Encrypt(JsonObject values, string secret);
}
