using System.Collections.Generic;
using CoreFX.Abstractions.Consts;
using TestAbstractions.App_Start;

namespace UnitTest.Hello8.Domain.SDK.App_Start
{
    public abstract class DerivedUnitTestBase : UnitTestBase
    {
        public override List<string> GetConfigPathList() => new List<string>
        {
            SvcConst.DefaultAppSettingsFile
        };
    }
}
