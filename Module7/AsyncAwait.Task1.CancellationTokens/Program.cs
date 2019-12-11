/*
 * Изучите код данного приложения для расчета суммы целых чисел от 0 до N, а затем
 * измените код приложения таким образом, чтобы выполнялись следующие требования:
 * 1. Расчет должен производиться асинхронно.
 * 2. N задается пользователем из консоли. Пользователь вправе внести новую границу в процессе вычислений,
 * что должно привести к перезапуску расчета.
 * 3. При перезапуске расчета приложение должно продолжить работу без каких-либо сбоев.
 */

using System;
using AOP.AsyncAwait.Task1.CancellationTokens.Calculator;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AOP.AsyncAwait.Task1.CancellationTokens;
using AOP.CacheLib;
using Autofac;
using Autofac.Core;
using Castle.DynamicProxy;

namespace AsyncAwait.Task1.CancellationTokens
{
    /// <summary>
    /// Represent the main program
    /// </summary>
    [ExcludeFromCodeCoverage]
    class Program
    {
        private static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SimpleCalculator>()
                .As<ICalculator>()
                .AsSelf();

            builder.RegisterType<CacheInterceptor>()
                .AsSelf();

            builder.RegisterType<Application>()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(ICalculator),
                        (pi, ctx) => 
                        {
                            var generator = new ProxyGenerator();
                            return generator.CreateInterfaceProxyWithTarget<ICalculator>(
                                ctx.Resolve<SimpleCalculator>(), new CacheInterceptor());
                        })
                );

            return builder.Build();
        }

        /// <summary>
        /// The Main method should not be changed at all.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CompositionRoot().Resolve<Application>().Run();
        }
    }
}