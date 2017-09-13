using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using repkg.Map;

namespace repkg.Tests
{
	public class FilePackageMapTests
	{
		public class s
			: FilePackageMapTests
		{
			[Test]
			public void Ignores_Comments()
			{
				var lines = new string[]
					{
						"# Comment line 1",
						"# Comment line 2",
						"DevExpress.BonusSkins @15.2.11.0 | DevExpress.Win.BonusSkins @17.1.6.0",
						"# Comment line 3"
					};

				var map = new FilePackageMap(new DirectLineReader(lines));
				map.ItemCount.Should().Be(1);
			}

			[Test]
			public void Ignores_Whitespace_Lines()
			{
				var lines = new string[]
					{
						"",
						"DevExpress.BonusSkins @15.2.11.0 | DevExpress.Win.BonusSkins @17.1.6.0",
						""
					};

				var map = new FilePackageMap(new DirectLineReader(lines));
				map.ItemCount.Should().Be(1);
			}

			[Test]
			public void Parses_1_To_1_Mappings()
			{
				var lines = new string[]
					{
						"DevExpress.BonusSkins @15.2.11.0 | DevExpress.Win.BonusSkins @17.1.6.0"
					};

				var map = new FilePackageMap(new DirectLineReader(lines));

				var mapItem = map.GetMapItemFor("DevExpress.BonusSkins");

				mapItem.OldPackage.Name.Should().Be("DevExpress.BonusSkins");
				mapItem.OldPackage.Version.Should().Be("15.2.11.0");

				mapItem.NewPackages.Single().Name.Should().Be("DevExpress.Win.BonusSkins");
				mapItem.NewPackages.Single().Version.Should().Be("17.1.6.0");

				mapItem.ShouldConvertPackage.Should().BeTrue();
				mapItem.ShouldRemovePackage.Should().BeFalse();
			}

			[Test]
			public void Parses_1_To_0_Mappings()
			{
				var lines = new string[]
					{
						"DevExpress.BonusSkins @15.2.11.0 | "
					};

				var map = new FilePackageMap(new DirectLineReader(lines));

				var mapItem = map.GetMapItemFor("DevExpress.BonusSkins");

				mapItem.OldPackage.Name.Should().Be("DevExpress.BonusSkins");
				mapItem.OldPackage.Version.Should().Be("15.2.11.0");

				mapItem.NewPackages.Count.Should().Be(0);

				mapItem.ShouldConvertPackage.Should().BeFalse();
				mapItem.ShouldRemovePackage.Should().BeTrue();
			}

			[Test]
			public void Parses_1_To_N_Mappings()
			{
				var lines = new string[]
					{
						"DevExpress.BonusSkins @15.2.11.0 | DevExpress.Win.BonusSkins @17.1.6.0 : DevExpress.Mac.BonusSkins @High Sierra"
					};

				var map = new FilePackageMap(new DirectLineReader(lines));

				var mapItem = map.GetMapItemFor("DevExpress.BonusSkins");

				mapItem.OldPackage.Name.Should().Be("DevExpress.BonusSkins");
				mapItem.OldPackage.Version.Should().Be("15.2.11.0");

				mapItem.NewPackages.Count.Should().Be(2);
				mapItem.NewPackages.First().Name.Should().Be("DevExpress.Win.BonusSkins");
				mapItem.NewPackages.First().Version.Should().Be("17.1.6.0");
				mapItem.NewPackages.Last().Name.Should().Be("DevExpress.Mac.BonusSkins");
				mapItem.NewPackages.Last().Version.Should().Be("High Sierra");

				mapItem.ShouldConvertPackage.Should().BeTrue();
				mapItem.ShouldRemovePackage.Should().BeFalse();
			}
		}
	}
}
