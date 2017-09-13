﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace repkg.Map
{
	[DebuggerDisplay("{OldPackageName,nq} @{OldVersion,nq} -> {NewPackageName,nq} @{NewVersion,nq}")]
	public class MapItem
	{
		public Package OldPackage { get; set; }

		public List<Package> NewPackages { get; set; }

		public bool HasOldVersion => OldPackage?.IsValid ?? false;

		public bool HasNewVersion => NewPackages != null
										&& NewPackages.Any()
										&& NewPackages.All(p => p.IsValid);

		public bool ShouldConvertPackage => HasOldVersion && HasNewVersion;

		public bool ShouldRemovePackage => HasOldVersion && !HasNewVersion;

		public class Package
		{
			public string Name { get; set;}

			public string Version { get; set; }

			public bool IsValid => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Version);
		}
	}
}
