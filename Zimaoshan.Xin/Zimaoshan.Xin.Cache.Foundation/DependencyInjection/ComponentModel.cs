using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Zimaoshan.Xin.Cache.Foundation.DependencyInjection
{
    /// <summary>
    /// 组件实体
    /// </summary>
    public class ComponentModel
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServiceType { get; set; } = default!;

        /// <summary>
        /// 生命周期
        /// </summary>
        public ServiceLifetime LifeScope { get; set; } = ServiceLifetime.Transient;

        /// <summary>
        /// 提供三种：接口、特性和方法名
        /// </summary>
        public LocationMode Mode { get; set; }

        /// <summary>
        /// ServiceType是否动态泛型类型
        /// https://github.com/yuzd/Autofac.Annotation/issues/13
        /// </summary>
        internal bool IsDynamicGeneric
        {
            get
            {
                return this.ServiceType.GetTypeInfo().IsGenericTypeDefinition;
            }
        }
    }

    /// <summary>
    /// 定位方式
    /// </summary>
    public enum LocationMode
    {
        Interface,
        Attribute,
        MethodName
    }
}
