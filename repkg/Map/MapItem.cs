using System.Diagnostics;

namespace repkg.Map
{
	[DebuggerDisplay("{OldPackageName,nq} @{OldVersion,nq} -> {NewPackageName,nq} @{NewVersion,nq}")]
	public class MapItem
	{
		public string OldPackageName { get; set; }
		public string OldVersion { get; set; }
		public string NewPackageName { get; set; }
		public string NewVersion { get; set; }

		public bool HasOldVersion => !string.IsNullOrEmpty(OldPackageName) && !string.IsNullOrEmpty(OldVersion);

		public bool HasNewVersion => !string.IsNullOrEmpty(NewPackageName) && !string.IsNullOrEmpty(NewVersion);

		public bool ShouldConvertPackage => HasOldVersion && HasNewVersion;

		public bool ShouldRemovePackage => HasOldVersion && !HasNewVersion;
	}
}
