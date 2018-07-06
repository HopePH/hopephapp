using System;

namespace Yol.Punla.Barrack
{
    public interface INavigationType
    {
        Uri CreateMasterNavigationType();
        Uri CreateFlatNavigationType();
    }
}
