using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.WindowsClient
{
    public class ResourceInterceptor : IResourceInterceptor
    {
        private string _token { get; set; }
        public ResourceInterceptor(string token)
        {
            _token = token;
        }
        public bool OnFilterNavigation(NavigationRequest request)
        {
            return false;
        }

        public ResourceResponse OnRequest(ResourceRequest request)
        {
            request.AppendExtraHeader("token", _token);
            return null;
        }
    }
}
