using Yol.Punla.AttributeBase;

namespace Yol.Punla.Messages
{
    [DefaultModule]
    public class AppMasterPageMessage
    {
        public bool IsOpen { get; set; }
        public bool IsShowSettings { get; set; }
    }
}
