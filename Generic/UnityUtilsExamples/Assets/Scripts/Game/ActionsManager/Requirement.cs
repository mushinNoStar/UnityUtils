using System.Collections.Generic;
using System;

namespace Actions
{
    /// <summary>
    /// a requirement is a object that can tell if some particular requirement are met.
    /// it is mostly used by actions.
    /// this should be indipendant from the action you want to perform.
    /// </summary>
    public abstract class Requirement
    {
        private static Dictionary<string, Type> requirementTypes = null;

        /// <summary>
        /// Check if the requirement is shatishied or not.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="targets"></param>
        /// <param name="otherArgs"></param>
        /// <returns></returns>
        public abstract bool isShatishied(Actor caller, List<Target> targets, List<string> otherArgs);

        /// <summary>
        /// return the error message it must write if it's not shatfishied.
        /// </summary>
        /// <returns></returns>
        public abstract string getMessage();

        /// <summary>
        /// return the type of a requirement by name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type getRequirementType(string typeName)
        {
            if (requirementTypes == null)
                loadTypes();
            return requirementTypes[typeName];
        }

        /// <summary>
        /// return every type in the assemblies
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Type> getTypes()
        {
            if (requirementTypes == null)
                loadTypes();
            return requirementTypes;
        }

        private static void loadTypes()
        {
            requirementTypes = new Dictionary<string, Type>();
            List<Type> listOfType = new List<Type>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.BaseType == typeof(Target))
                        listOfType.Add(type);
                }
            }
            foreach (Type t in listOfType)
            {
                if (t.IsAbstract)
                    continue;
                requirementTypes.Add(t.Name, t);
            }
        }
    }
}