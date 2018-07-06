using Yol.Punla.AttributeBase;

namespace Yol.Punla.FakeEntries
{
    [DefaultModuleFake]
    public class FakeLoadingMorePost
    {
        public bool IsNotExpiredTime { get; set; }
        public bool IsDuplicatePost { get; set; }
    }
}
