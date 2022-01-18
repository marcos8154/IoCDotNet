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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCdotNet
{
    /// <summary>
    /// Support class for accessing IoC services
    /// </summary>
    public sealed class IoC
    {
        private static InstanceManager _instance = null;

        /// <summary>
        /// Gets an instance of the bindings manager.
        /// </summary>
        /// <returns></returns>
        public static InstanceManager GetManager()
        {
            if (_instance == null)
                _instance = new InstanceManagerImpl();
            return _instance;
        }

        /// <summary>
        /// Obtains the instance of an implementation through the alias
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="alias">Alias identifier for one of the possible implementations</param>
        /// <param name="args">Constructor arguments for concrete implementation. If no parameter is entered, the default constructor will be used</param>
        /// <returns></returns>
        public static T ResolveNamed<T>(string alias, params object[] args)
        {
            return _instance.ResolveNamed<T>(alias, args);
        }


        /// <summary>
        /// Obtains the instance of an implementation through the type
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="implementation">The type of a unique, interface-specific implementation</param>
        /// <param name="args">Constructor arguments for concrete implementation. If no parameter is entered, the default constructor will be used</param>
        /// <returns></returns>
        public static T ResolveTyped<T>(Type implementation, params object[] args)
        {
            return _instance.ResolveTyped<T>(implementation, args);
        }
    

        /// <summary>
        /// Obtains the instance of an implementation through the alias
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="args">Constructor arguments for concrete implementation. If no parameter is entered, the default constructor will be used</param>
        /// <returns></returns>
        public static T Resolve<T>(params object[] args)
        {
            return GetManager().Resolve<T>(args);
        }
    }
}
