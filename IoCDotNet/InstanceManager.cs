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
    /// Interface for allocating and managing instances of other interfaces (IoC)
    /// </summary>
    public interface InstanceManager
    {
        /// <summary>
        /// Bind the interface and implementation through Object Type
        /// </summary>
        /// <param name="interfaceType">Interface object Type</param>
        /// <param name="implementationType">Implementation object Type</param>
        /// <param name="makeSingleton">Make this bind singleton. When the interface instance is requested, an in-memory instance of the implementation will be returned.</param>
        void Bind(Type interfaceType, Type implementationType, bool makeSingleton);

        /// <summary>
        /// Bind the interface and implementation through Object Type
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="interfaceType">Interface object Type</param>
        /// <param name="implementationType">Implementation object Type</param>
        /// <param name="makeSingleton">Make this bind singleton. When the interface instance is requested, an in-memory instance of the implementation will be returned.</param>
        void Bind(string alias, Type interfaceType, Type implementationType, bool makeSingleton);

        /// <summary>
        /// Binds an interface to a single implementation possibility
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="implementation">Concrete type of interface implementation</param>
        /// <param name="singletonInstance">Determines whether the instance of the implementation should be "singleton"</param>
        void Bind<T>(Type implementation, bool singletonInstance);

        /// <summary>
        /// Binds an interface to a multiple implementations possibilities
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="alias">A simplified alias to locate one of the possible concrete instances of the interface</param>
        /// <param name="implementation">Concrete type of interface implementation</param>
        /// <param name="singletonInstance">Determines whether the instance of the implementation should be "singleton"</param>
        void Bind<T>(string alias, Type implementation, bool singletonInstance);


        /// <summary>
        /// Unbind all bindings from interface
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        void Unbind<T>();

        /// <summary>
        /// Unbind interface bindings from alias name
        /// </summary>
        /// <param name="alias">Alias name for binding</param>
        void Unbind(string alias);

        /// <summary>
        /// Unbind interface bindings by implementation concrete type
        /// </summary>
        /// <param name="implementation"></param>
        void Unbind(Type implementation);



        /// <summary>
        /// Obtains the instance of an implementation through the alias
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="args">Constructor arguments for concrete implementation. If no parameter is entered, the default constructor will be used</param>
        /// <returns></returns>
        T Resolve<T>(params object[] args);


        /// <summary>
        /// Obtains the instance of an implementation through the type
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="implementation">The type of a unique, interface-specific implementation</param>
        /// <param name="args">Constructor arguments for concrete implementation. If no parameter is entered, the default constructor will be used</param>
        /// <returns></returns>
        T ResolveTyped<T>(Type implementation, params object[] args);


        /// <summary>
        /// Obtains the instance of an implementation through the alias
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="alias">Alias identifier for one of the possible implementations</param>
        /// <param name="args">Constructor arguments for concrete implementation. If no parameter is entered, the default constructor will be used</param>
        /// <returns></returns>
        T ResolveNamed<T>(string alias, params object[] args);


        /// <summary>
        /// List all binding services from current service manager instance
        /// </summary>
        /// <returns>List of ManagedServiceBinding</returns>
        IReadOnlyCollection<ManagedInstanceBinding> BindingCollection();
    }
}
