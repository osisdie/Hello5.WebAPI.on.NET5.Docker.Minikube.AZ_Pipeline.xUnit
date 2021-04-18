using System.Collections.Generic;
using CoreFX.Abstractions.Consts;
using TestAbstractions.App_Start;

namespace UnitTest.CoreFX.Caching.Redis.App_Start
{
    public abstract class DerivedUnitTestBase : UnitTestBase
    {
        public override List<string> GetConfigPathList() => new List<string>
        {
            SvcConst.DefaultAppSettingsFile
        };
    }
}
