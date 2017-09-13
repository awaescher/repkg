using FluentAssertions;
using NUnit.Framework;
using repkg.Map;
using System;
using System.Collections.Generic;

namespace repkg.Tests
{
	public class MapItemTests
	{
		public class HasOldVersionProperty
			: MapItemTests
		{
			[Test]
			public void Returns_False_When_Name_And_Version_Are_Not_Set()
			{
				var item = new MapItem()
				{
					OldPackage = new MapItem.Package()
				};

				item.HasOldVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_False_When_Only_Name_Is_Set()
			{
				var item = new MapItem()
				{
					OldPackage = new MapItem.Package()
					{
						Name = "my package"
					}
				};

				item.HasOldVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_False_When_Only_Version_Is_Set()
			{
				var item = new MapItem()
				{
					OldPackage = new MapItem.Package()
					{
						Version = "1.0"
					}
				};

				item.HasOldVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_True_When_Name_And_Version_Are_Set()
			{
				var item = new MapItem()
				{
					OldPackage = new MapItem.Package()
					{
						Name = "my package",
						Version = "1.0"
					}
				};

				item.HasOldVersion.Should().BeTrue();
			}
		}

		public class HasNewVersionProperty
			: MapItemTests
		{
			[Test]
			public void Returns_False_When_NewPackages_Is_Null()
			{
				var item = new MapItem()
				{
					NewPackages = null
				};

				item.HasNewVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_False_When_NewPackages_Is_An_Empty_List()
			{
				var item = new MapItem()
				{
					NewPackages = new List<MapItem.Package>()
				};

				item.HasNewVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_False_When_A_Package_Is_Given_Without_Name_Or_Version()
			{
				var item = new MapItem()
				{
					NewPackages = new List<MapItem.Package>()
					{
						new MapItem.Package()
					}
				};

				item.HasNewVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_False_When_A_Package_Is_Given_With_A_Name_Only()
			{
				var item = new MapItem()
				{
					NewPackages = new List<MapItem.Package>()
					{
						new MapItem.Package()
						{
							Name = "my package"
						}
					}
				};

				item.HasNewVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_False_When_A_Package_Is_Given_With_A_Version_Only()
			{
				var item = new MapItem()
				{
					NewPackages = new List<MapItem.Package>()
					{
						new MapItem.Package()
						{
							Version = "1.0"
						}
					}
				};

				item.HasNewVersion.Should().BeFalse();
			}

			[Test]
			public void Returns_True_When_Name_And_Version_Are_Set()
			{
				var item = new MapItem()
				{
					NewPackages = new List<MapItem.Package>()
					{
						new MapItem.Package()
						{
							Name = "my package",
							Version = "1.0"
						}
					}
				};


				item.HasNewVersion.Should().BeTrue();
			}
		}
	}
}
