﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
// ReSharper disable InconsistentNaming

namespace TgDownloaderConsole.Helpers;

internal partial class TgMenuHelper
{
	#region Public and private methods

	public bool CheckTgSettingsWithWarning(TgDownloadSettingsModel tgDownloadSettings)
	{
		bool result = TgClient is { IsReady: true } && tgDownloadSettings.IsReady;
		if (!result)
		{
			TgLog.MarkupWarning(TgLocale.TgMustSetSettings);
			Console.ReadKey();
		}
		return result;
	}

	public void RunAction(TgDownloadSettingsModel tgDownloadSettings, 
		Action<TgDownloadSettingsModel, Action<string, bool>> action, bool isSkipCheckTgSettings, bool isScanCount)
	{
		if (!isSkipCheckTgSettings && !CheckTgSettingsWithWarning(tgDownloadSettings))
			return;

		AnsiConsole.Status()
			.AutoRefresh(true)
			.Spinner(Spinner.Known.Star)
			.SpinnerStyle(Style.Parse("green"))
			.Start("Thinking...", statusContext =>
			{
				Stopwatch sw = new();
				sw.Start();
				action(tgDownloadSettings, RefreshStatus);
				sw.Stop();
				// TgClient.DicChatsAll.Count
				if (isScanCount)
					RefreshStatus($"{GetStatus(sw, tgDownloadSettings.SourceScanCount, tgDownloadSettings.SourceScanCurrent)}", false);
				else
					RefreshStatus($"{GetStatus(sw, tgDownloadSettings.SourceFirstId, tgDownloadSettings.SourceLastId)}", false);
				// Callback for refresh status.
				void RefreshStatus(string message, bool isProgress)
				{
					if (isScanCount)
						statusContext.Status(isProgress
							? TgLog.GetMarkupString($"{GetStatus(tgDownloadSettings.SourceScanCount, 
								tgDownloadSettings.SourceScanCurrent)} | {message}")
							: TgLog.GetMarkupString(message));
					else
						statusContext.Status(isProgress
							? TgLog.GetMarkupString($"{GetStatus(tgDownloadSettings.SourceLastId, 
								tgDownloadSettings.SourceFirstId)} | {message}")
							: TgLog.GetMarkupString(message));
					statusContext.Refresh();
				}
			});
		TgLog.MarkupLine(TgLocale.TypeAnyKeyForReturn);
		Console.ReadKey();
	}

	private double CalcSourceProgress(long count, long current) =>
		count is 0 ? 0 : (double)(current * 100) / count;

	private string GetLongString(long current) =>
		current > 999 ? $"{current:### ###}" : $"{current:###}";

	private string GetStatus(Stopwatch sw, long count, long current) =>
		count is 0 && current is 0
			? $"{TgLog.GetDtShortStamp()} | {sw.Elapsed} | "
			: $"{TgLog.GetDtShortStamp()} | {sw.Elapsed} | {CalcSourceProgress(count, current):#00.00} % | {GetLongString(current)} / {GetLongString(count)}";

	private string GetStatus(long count, long current) =>
		count is 0 && current is 0
			? TgLog.GetDtShortStamp()
			: $"{TgLog.GetDtShortStamp()} | {CalcSourceProgress(count, current):#00.00} % | {GetLongString(current)} / {GetLongString(count)}";

	#endregion
}