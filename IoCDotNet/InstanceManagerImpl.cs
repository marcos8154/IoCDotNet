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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCdotNet
{
    internal sealed class InstanceManagerImpl : InstanceManager
    {
        private object lckObj = new object();

        private List<ManagedInstanceBinding> Bindings { get; set; }

        public InstanceManagerImpl()
        {
            Bindings = new List<ManagedInstanceBinding>();
        }

        public void Bind(Type interfaceType, Type implementationType, bool makeSingleton)
        {
            if (!interfaceType.IsInterface)
                throw new Exception("'T' is not a interface.");

            lock (lckObj)
                Bindings.Add(new ManagedInstanceBinding(interfaceType, implementationType, makeSingleton));
        }

        public void Bind(string alias, Type interfaceType, Type implementationType, bool makeSingleton)
        {
            if (!interfaceType.IsInterface)
                throw new Exception($"The Type '{interfaceType.FullName}' is not an Interface.");

            lock (lckObj)
                Bindings.Add(new ManagedInstanceBinding(alias, interfaceType, implementationType, makeSingleton));
        }

        public void Bind<T>(Type implementationType, bool makeItSingleton)
        {
            if (!typeof(T).IsInterface)
                throw new Exception($"The Type '{typeof(T).FullName}' is not an Interface.");
            lock (lckObj)
                Bindings.Add(new ManagedInstanceBinding(typeof(T), implementationType, makeItSingleton));
        }

        public void Bind<T>(string alias, Type implementation, bool makeSingleton)
        {
            if (!typeof(T).IsInterface)
                throw new Exception($"The Type '{typeof(T).FullName}' is not an Interface.");
            lock (lckObj)
                Bindings.Add(new ManagedInstanceBinding(alias, typeof(T), implementation, makeSingleton));
        }

        public void Unbind<T>()
        {
            Type interfaceType = typeof(T);
            var bindings = Bindings.Where(b => b.InterfaceType.Equals(interfaceType)).ToList();
            if (bindings == null)
                return;

            lock (lckObj)
            {
                bindings.ForEach(b => b.DestroySingleton());
                Bindings.RemoveAll(b => b.InterfaceType.Equals(interfaceType));
            }
        }

        public void Unbind(string alias)
        {
            var bind = Bindings.FirstOrDefault(b => alias.Equals(b.Name));
            if (bind == null)
                return;

            lock (lckObj)
            {
                bind.DestroySingleton();
                Bindings.Remove(bind);
                bind = null;
            }
        }

        public void Unbind(Type implementation)
        {
            var bind = Bindings.FirstOrDefault(b => b.Implementation.Equals(implementation));
            if (bind == null)
                return;

            lock (lckObj)
            {
                bind.DestroySingleton();
                Bindings.Remove(bind);
                bind = null;
            }
        }

        public T ResolveNamed<T>(string alias, params object[] args)
        {
            try
            {
                var bind = Bindings.FirstOrDefault(b => alias.Equals(b.Name));
                if (bind == null)
                    return default;

                return ResolveInternal<T>(bind, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T ResolveTyped<T>(Type implementation, params object[] args)
        {
            try
            {
                var bind = Bindings.FirstOrDefault(b => b.Implementation.Equals(implementation));
                if (bind == null)
                    return default;

                return ResolveInternal<T>(bind, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T Resolve<T>(params object[] args)
        {
            try
            {
                var bind = Bindings.FirstOrDefault(b => b.InterfaceType.Equals(typeof(T)) &&
                    string.IsNullOrEmpty(b.Name));
                if (bind == null)
                    return default;
                return ResolveInternal<T>(bind, args);
            }
            catch
            {
                return default;
            }
        }

        public IReadOnlyCollection<ManagedInstanceBinding> BindingCollection()
        {
            return Bindings.ToList().AsReadOnly();
        }

        public override string ToString()
        {
            return "InstanceManager";
        }

        private T ResolveInternal<T>(ManagedInstanceBinding bind, params object[] args)
        {
            if (bind.Singleton)
            {
                bind.InitializeSingleton(args);
                return (T)bind.SingletonInstance;
            }

            object concreteInstance = Activator.CreateInstance(bind.Implementation, args);
            return (T)concreteInstance;
        }
    }
}
