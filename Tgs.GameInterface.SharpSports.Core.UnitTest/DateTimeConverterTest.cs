namespace Tgs.GameInterface.SharpSports.Core.UnitTest;

public sealed class DateTimeConverterTest
{
	[Fact]
	public void TestStringConvertToDatetime()
	{
		var sut = new DateTimeConverter();

		var actual = sut.StringConvertToDatetime("2024-06-04 13:12:11");

		Assert.Equal(new DateTimeOffset(2024, 06, 04, 13, 12, 11, offset: TimeSpan.FromHours(8)), actual);
	}

	[Fact]
	public void TestStringConvertToDatetime_WhenWrongFormat_ReturnUnixEpoch()
	{
		var sut = new DateTimeConverter();

		var actual = sut.StringConvertToDatetime("abcdefg");

		Assert.Equal(DateTimeOffset.UnixEpoch, actual);
	}

	[Fact]
	public void TestStringConvertToDatetime_WhenEmptyString_ReturnUnixEpoch()
	{
		var sut = new DateTimeConverter();

		var actual = sut.StringConvertToDatetime(string.Empty);

		Assert.Equal(DateTimeOffset.UnixEpoch, actual);
	}

	[Fact]
	public void TestStringConvertToDatetime_WhenDiffFormat()
	{
		var sut = new DateTimeConverter();

		var dateTime = new DateTimeOffset(2024, 06, 04, 05, 12, 11, TimeSpan.Zero);
		var actual = sut.StringConvertToDatetime(dateTime.ToString("u"));

		Assert.Equal(new DateTimeOffset(2024, 06, 04, 13, 12, 11, offset: TimeSpan.FromHours(8)), actual);
	}

	[Fact]
	public void TestDatetimeConvertToString()
	{
		var sut = new DateTimeConverter();

		var actual = sut.DatetimeConvertToString(new DateTimeOffset(2024, 06, 04, 13, 12, 11, offset: TimeSpan.FromHours(8)));

		Assert.Equal("2024-06-04 13:12:11", actual);
	}

	[Fact]
	public void TestDatetimeConvertToString_WhenDiffTimeZone()
	{
		var sut = new DateTimeConverter();

		var actual = sut.DatetimeConvertToString(new DateTimeOffset(2024, 06, 04, 13, 12, 11, offset: TimeSpan.FromHours(9)));

		Assert.Equal("2024-06-04 12:12:11", actual);
	}
}
