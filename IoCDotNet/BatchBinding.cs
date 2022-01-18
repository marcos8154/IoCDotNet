/*
MIT License

Copyright (c) 2022 Marcos Vinícius

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using IoCdotNet;
using IoCdotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IoCdotNet
{

    /// <summary>
    /// Allows you to immediately link batches of interfaces and implementations, based on the Namespace path between them.
    /// </summary>
    public class BatchBinding
    {
        /// <summary>
        ///  Allows you to immediately link batches of interfaces and implementations, based on the Namespace path between them. <br/>
        /// 
        /// For each interface, its respective implementation will be located within indicated namespace. <br/><br/>
        /// So Magical ❤
        /// </summary>
        /// <param name="baseAssembly">Target assembly-object</param>
        /// <param name="interfacesNamespace">Namespace in which a set of interfaces is located</param>
        /// <param name="implementationsNamespace">Namespace in which an implementation set of the interfaces that are in 'interfacesNamespace' is located</param>
        public static void MagicBind(Assembly baseAssembly,
            string interfacesNamespace,
            string implementationsNamespace)
        {
            InstanceManager manager = IoC.GetManager();

            Type[] interfaces = baseAssembly.GetTypes()
                .Where(t => t != null && t.IsInterface && interfacesNamespace.Equals(t.Namespace))
                .ToArray();

            Type[] implementations = baseAssembly.GetTypes()
                .Where(t => t != null && !t.IsInterface && implementationsNamespace.Equals(t.Namespace))
                .ToArray();

            foreach (Type interfaceType in interfaces)
            {
                if (interfaceType.Name.Contains(">"))
                    continue;
                if (!interfaceType.IsInterface)
                    continue;

                List<Type> implementationTypes = implementations
                      .Where(t => interfaceType.Equals(t.GetInterface(interfaceType.Name)))
                      .ToList();

                foreach (Type implementationType in implementationTypes)
                {
                    if (implementationType.Name.Contains(">"))
                        continue;

                    bool singleton = implementationType.GetCustomAttribute(typeof(SingletonInstance)) != null;
                    Attribute namedAttribute = implementationType.GetCustomAttribute(typeof(NamedInstance));
                    if (namedAttribute == null)
                        manager.Bind(interfaceType, implementationType, singleton);
                    else
                    {
                        string serviceName = ((NamedInstance)namedAttribute).InstanceName;
                        manager.Bind(serviceName, interfaceType, implementationType, singleton);
                    }
                }
            }
        }
    }
}
