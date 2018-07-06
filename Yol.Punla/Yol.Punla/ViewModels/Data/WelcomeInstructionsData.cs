using System.Collections.Generic;
using Yol.Punla.Entity;
using Yol.Punla.Localized;

namespace Yol.Punla.ViewModels.Data
{
    public static class WelcomeInstructionsData
    {
        private static IEnumerable<WelcomeInstruction> WelcomeInstructionsField;
        public static IEnumerable<WelcomeInstruction> Instructions { get => WelcomeInstructionsField; set => WelcomeInstructionsField = value; }

        public static void Init()
        {
            WelcomeInstructionsField = new List<WelcomeInstruction>
            {
                new WelcomeInstruction { TextInstruction = AppStrings.WelcomeInstructions1, ImageInstruction ="ins1.png" },
                new WelcomeInstruction { TextInstruction = AppStrings.WelcomeInstructions2, ImageInstruction ="ins2.png" },
                new WelcomeInstruction { TextInstruction = AppStrings.WelcomeInstructions3, ImageInstruction ="ins3.png" },
                new WelcomeInstruction { TextInstruction = AppStrings.WelcomeInstructions4, ImageInstruction ="ins4.png" }
            };
        }
    }
}
