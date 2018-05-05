using Framework.Core.IoC;
using Microsoft.Extensions.Caching.Memory;

namespace Framework.Core.Cache
{
    public class MemoryCacheManager
    {
        public static IMemoryCache GetInstance() => AspectCoreContainer.Resolve<IMemoryCache>();
    }
}
