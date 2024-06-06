namespace Tgs.GameInterface.SharpSports.Core;

internal sealed class DateTimeConverter
{
	private readonly TimeZoneInfo _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "China Standard Time", baseUtcOffset: TimeSpan.FromHours(+8), "China Standard Time", "China Standard Time");

	private readonly string _toStringformat = "yyyy-MM-dd HH:mm:ss";

	public string DatetimeConvertToString(DateTimeOffset time)
		=> TimeZoneInfo.ConvertTime(time.ToUniversalTime(), _timeZone).ToString(_toStringformat);

	public DateTimeOffset StringConvertToDatetime(string str)
		=> DateTime.TryParse(str, out var date) ? new(date, _timeZone.GetUtcOffset(date)) : DateTimeOffset.UnixEpoch;
}
