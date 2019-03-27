using System;

namespace Zyrenth.Zora.Tests
{
	public class TestSecret : Secret
	{
		public override int Length => throw new NotImplementedException();

		public override void Load(byte[] secret, GameRegion region)
		{
			throw new NotImplementedException();
		}

		public override byte[] ToBytes()
		{
			throw new NotImplementedException();
		}
	}
}
