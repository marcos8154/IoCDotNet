using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCdotNet.Attributes
{
    /// <summary>
    /// A named instance allows the same interface to have several easily accessible implementations. <br/>
    /// 
    /// To request it, 'IoC.ResolveNamed<![CDATA[<T>]]>(...)' must be invoked
    /// </summary>
    public class NamedInstance : Attribute
    {
        public string InstanceName { get; set; }

        public NamedInstance(string instanceName)
        {
            InstanceName = instanceName;
        }
    }
}
