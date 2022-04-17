using Photon.Pun;

namespace Game.Setup.Photon
{
    public class PhotonCallbackHandler : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var player = GetComponent<Player>();
            if (player == null)
            {
                return;
            }
            var instantiationData = info.photonView.InstantiationData;
            if (instantiationData.Length == 0)
            {
                return;
            }
            var playerName = instantiationData[0] as string;
            if (string.IsNullOrEmpty(playerName))
            {
                return;
            }
            player.SetName(playerName);
        }
    }
}
