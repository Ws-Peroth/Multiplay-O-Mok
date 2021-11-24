using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Game.PhotonStreaming
{
    public static class EventSender
    {
        public static void SendRaiseEvent(EventTypes eventCode, object contents, ReceiverGroup sendTarget)
        {
            SendRaiseEvent((byte) eventCode, contents, sendTarget);
        }

        private static void SendRaiseEvent(byte eventCode, object contents, ReceiverGroup sendTarget)
        {
            var raiseEventOptions = new RaiseEventOptions()
            {
                Receivers = sendTarget
            };
            var sendOptions = new SendOptions()
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent(eventCode, contents, raiseEventOptions, sendOptions);
        }
    }
}