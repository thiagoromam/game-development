using System;

namespace Exercise3.Agent.States
{
    public static class Extensions
    {
        public static IState CreateBuildStructState(Type genericType)
        {
            return (IState)Activator.CreateInstance(typeof(BuildStructureState<>).MakeGenericType(genericType));
        }
    }
}