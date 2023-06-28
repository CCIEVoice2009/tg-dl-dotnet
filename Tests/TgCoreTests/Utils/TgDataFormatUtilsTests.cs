﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace TgCoreTests.Utils;

[TestFixture]
public class TgDataFormatUtilsTests
{
	#region Public and private methods

	[Test]
	public void DataFormatUtils_CheckFileAtMask_AreEqual()
	{
		Assert.DoesNotThrow(() =>
		{
			string fileName = "RW-Kotlin-Cheatsheet-1.1.pdf";
			bool result = TgDataFormatUtils.CheckFileAtMask(fileName, "kotlin");
			Assert.True(result);
			result = TgDataFormatUtils.CheckFileAtMask(fileName, "PDF");
			Assert.True(result);

			fileName = "C# Generics.ZIP";
			result = TgDataFormatUtils.CheckFileAtMask(fileName, "c*#");
			Assert.True(result);
			result = TgDataFormatUtils.CheckFileAtMask(fileName, "zip");
			Assert.True(result);
		});
	}

	#endregion
}