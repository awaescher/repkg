namespace repkg.Map
{
	public interface IPackageMap
	{
		MapItem GetMapItemFor(string package);

		int ItemCount { get; }
	}


}