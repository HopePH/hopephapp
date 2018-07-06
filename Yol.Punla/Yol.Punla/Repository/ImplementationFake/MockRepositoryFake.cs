using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IMockRepository))]
    public class MockRepositoryFake : IMockRepository
    {
        public void CreateTablesOnce()
        {
           
        }
    }
}
