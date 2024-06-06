using System.Text.Json.Nodes;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest;

public sealed class ParameterEncryptorTest
{
	[Fact]
	public void TestEncryptManualSample()
	{
		/*
这是一个php版本的例子代码

// 要加密的原始数据
$data = '{"loginName":"test333333"}';

// 加密所需的密钥
$key = "7b0cc344486341559fa0252f7065c406";

$key = substr(openssl_digest(openssl_digest($key, 'sha1', true), 'sha1', true), 0, 16);
$out = openssl_encrypt($data, 'AES-128-ECB', $key, OPENSSL_RAW_DATA);

$data = strtoupper(bin2hex($out));

echo $data;
// DF41D5988EF382D37584D1E01EC80B10F8DEB236FE5171BB091AEBBE2201EDCF
		*/

		var encryptor = new ParameterEncryptor();
		var secret = "7b0cc344486341559fa0252f7065c406";

		var actual = encryptor.Encrypt(new JsonObject
		{
			["loginName"] = "test333333"
		}, secret);

		Assert.Equal("DF41D5988EF382D37584D1E01EC80B10F8DEB236FE5171BB091AEBBE2201EDCF", actual);
	}
}
