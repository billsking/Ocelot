using System.Collections.Generic;
using System.Net.Http;

namespace Ocelot.Configuration.File
{
    public class BackendRoute
    {
        public string Scheme { get; set; }
        public int? Port { get; set; }
        public string Host { get; set; }
        public List<BackendPathAndMethod> BackendPathAndMethods { get; set; }
    }

    public class BackendPathAndMethod
    {
        public HttpMethod Method { get; set; }
        public string Path { get; set; }

        /// <summary>
        /// 用于路由匹配的关键字
        /// </summary>
        public string MapKey { get; set; }
    }
}
