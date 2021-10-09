using Networking;
using Photon.Pun;

namespace Objects
{
	public class PlayerHealth : AnimatedHealth
	{
		private PlayerSetup _playerSetup;

		protected override void Start()
		{
			base.Start();

			_playerSetup = GetComponent<PlayerSetup>();
		}

		protected override void OnDeath()
		{
			PhotonNetwork.DestroyPlayerObjects(_playerSetup.GetActorNumber());
		}
	}
}