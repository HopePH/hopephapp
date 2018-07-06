using Yol.Punla.AttributeBase;

namespace Yol.Punla.Barrack
{
    [DefaultModuleFake]
    [DefaultModule]
    public class Advertisements 
    {
        public bool HasShownBetaMessageAlready { get; set; }
        public bool HasShownPullDownInstructionAlready { get; set; }
    }
}
