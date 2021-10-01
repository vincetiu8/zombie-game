namespace Networking
{
	public interface INetworkSerializeView
	{
		bool Serialize(byte[] data, ref int offset);

		void Deserialize(byte[] data, ref int offset);
	}
}