using System;
using System.Reflection;
using Castle.DynamicProxy;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;
using NHibernate;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace GRG.LeisureCards.WebAPI.Interceptors
{

    /// <summary>
    /// This interceptor is used to manage transactions.
    /// </summary>
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Creates a new UnitOfWorkInterceptor object.
        /// </summary>
        /// <param name="sessionFactory">Nhibernate session factory.</param>
        public UnitOfWorkInterceptor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>
        /// Intercepts a method.
        /// </summary>
        /// <param name="invocation">Method invocation arguments</param>
        public void Intercept(IInvocation invocation)
        {
            //If there is a running transaction, just run the method
            if (UnitOfWork.Current != null || !RequiresDbConnection(invocation.MethodInvocationTarget))
            {
                invocation.Proceed();
                return;
            }

            try
            {
                UnitOfWork.Current = new UnitOfWork(_sessionFactory);
                UnitOfWork.Current.BeginTransaction();

                try
                {
                    invocation.Proceed();
                    UnitOfWork.Current.Commit();
                }
                catch
                {
                    try
                    {
                        UnitOfWork.Current.Rollback();
                    }
                    catch
                    {

                    }

                    throw;
                }
            }
            finally
            {
                UnitOfWork.Current = null;
            }
        }

        private static bool RequiresDbConnection(MethodInfo methodInfo)
        {
            var value = (HasUnitOfWorkAttribute(methodInfo) || IsRepositoryMethod(methodInfo));

            return value;
        }

        public static bool IsRepositoryMethod(MethodInfo methodInfo)
        {
            return IsRepositoryClass(methodInfo.DeclaringType);
        }

        public static bool IsRepositoryClass(Type type)
        {
            var result = typeof(IRepository).IsAssignableFrom(type);

            return result;
        }

        public static bool HasUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }
    }
 }