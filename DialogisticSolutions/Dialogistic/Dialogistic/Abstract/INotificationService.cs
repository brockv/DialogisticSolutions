namespace Dialogistic.Abstract
{
    public interface INotificationService
    {
        void SendText(string to, string message);
        void MakePhoneCall(string to, string voiceUrl);
    }
}
